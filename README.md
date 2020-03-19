# Generate QR Code in an SSRS report with the Swiss QRCode library

## Overview
These are the steps required to create an SSRS report that displays QR code symbols using the Swiss QRCoder library

1. Obtain existing or compile the three QRCoder assemblies with strong names and the PartiallyTrustedCallers attribute*
2. Install the Three assemblies to the global assembly cache (GAC) using the gacutil.exe utility
3. Create a new SSRS report that queries a table of test data
4. Allow FileIOPermission
5. Add a reference to the SwissQRBillGenerator GAC assembly 
6. Add a custom code function that sends bill data to the SwissQRBillGenerator assembly and accepts a byte array in return
7. Add a field to the report and increase the physical dimensions of the field to accomodate a larger QR code symbol
8. Drag-and-drop an image componenet to the new field to bring up the image properties dialogue
9. Change the image source to Database, add a function that references the custom code function and change the MIME type to image/png
10. Set the size property of the image component to fill the available field space while maintaining the original aspect ratio
11. Execute the report to display the data and the QR code generated from the data

*A strong name is required to insert an assemby into the GAC. SSRS requires assemblies it references to include the PartiallyTrustedCallers attribute.*

## Resource File Content

![](Screens/Resource%20File%20Content.png)

The QRCodeGenerator, SwissBillQR.Net, and SwissQRBillGenerator folders contains C# projects used to compile the QR Coder demo application and assemblies. (You can load these projects into Visual Studio and compile them yourself if you wish.)

DLLs folder contains the assemblies that will be used to generate QR Codes in an SSRS report.

## Assemblies used to generate QR Code symbols in SSRS reports

Browse to the DLLs folder.

![](Screens/DLL%20File%20Content.png)

The QrCodeGenerator.dll assembly can generate QR Code symbols from an input string in a variety of image formats including svn.

The SwissBillQR.Net.dll assembly can generate Swiss QR Code symbols from an input string in a variety of image formats including svn using QrCodeGenerator.dll

SQL Server Reporting Services cannot display images directly, however, but requires images to be streamed as byte arrays. To address this, the SwissQRBillGenerator.dll assembly passes an input string to the generateQRcode() function of the SwissBillQR.Net.dll assembly and streams the returned bitmap image as a byte array to SSRS.

## Install the assemblies to the global assembly cache (GAC)

Find gacutil.exe with Agent Ransack or File Explorer.

![](Screens/Search%20GaUtil.png)

Copy the path of the appropriate 32- or 64-bit version of gacutil.exe:

Open a command console as administrator and change to the DLLs folder.

Install the assemblies to the GAC with the following commands:

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\gacutil.exe" -i QrCodeGenerator.dll

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\gacutil.exe" -i SwissBillQR.Net.dll

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\gacutil.exe" -i SwissBillQRcodeGenerator.dll

![](Screens/Install%20Assemblies.png)

Confirm the assemblies have been added to the C:\Windows\Microsoft.NET\assembly\GAC_MSIL folder.

![](Screens/GAC%20MSIL%20Status.png)

## Create test data for the report

Open a new query window in SQL Server Management Studio and paste the following SQL code into it.

```
BEGIN TRY
    DROP DATABASE TestDB
END TRY
BEGIN CATCH
END CATCH
GO
BEGIN TRY
    CREATE DATABASE TestDB
END TRY
BEGIN CATCH
END CATCH
GO
USE TestDB
BEGIN TRY
    DROP TABLE TestData
END TRY
BEGIN CATCH
END CATCH
GO
CREATE TABLE [dbo].[TestData](
    [Account] [nvarchar](50) NULL,
    [CreditorName] [nvarchar](50) NULL,
    [CreditorAddressLine1] [nvarchar](50) NULL,
    [CreditorAddressLine2] [nvarchar](50) NULL,
    [CreditorCountryCode] [nvarchar](50) NULL,
    [Amount] [Decimal](5,2),
    [Currency] [nvarchar](50) NULL,
    [DebtorName] [nvarchar](50) NULL,
    [DebtorAddressLine1] [nvarchar](50) NULL,
    [DebtorAddressLine2] [nvarchar](50) NULL,
    [DebtorCountryCode] [nvarchar](50) NULL,
    [Reference] [nvarchar](50) NULL,
    [UnstructuredMessage] [nvarchar](50) NULL
) ON [PRIMARY]
GO
INSERT INTO [dbo].[TestData] (Account,CreditorName,CreditorAddressLine1,CreditorAddressLine2,CreditorCountryCode,Amount,Currency,DebtorName,DebtorAddressLine1,DebtorAddressLine2,DebtorCountryCode,Reference,UnstructuredMessage) VALUES ('CH4431999123000889012','Robert Schneider AG','Rue du Lac 1268/2/22','2501 Biel','CH',199.95,'CHF','Pia-Maria Rutschmann-Schnyder','Grosse Marktgasse 28','9400 Rorschach','CH','210000000003139471430009017','Abonnement für 2020')
SELECT * FROM [dbo].[TestData]
```
Execute the query to create a database named TestDB and a table named TestData that contains some bill data.

## Allow FileIOPermission

Go to Control Panel / Administrative Tools / Microsoft .NET Framework
Configuration / configure security settings.
Then to My Computer->Runtime Security Policy node on the left and clicking
"Adjust Zone Security" link on the right side , give it full trust- give full trust to the Local Intranet also.And the error gine.

## Create and execute the QRCoder Demo SSRS report

Start a new Report Server Project Wizard in Visual Studio.

![](Screens/Create%20Report%20Wizad.png)

Insert the appropriate connection string information.

![](Screens/Add%20Data%20Source.png)

Insert the following code into the Query string text box.

`SELECT Account, CreditorName, CreditorAddressLine1, CreditorAddressLine2, CreditorCountryCode, Amount, Currency, DebtorName, DebtorAddressLine1, DebtorAddressLine2, DebtorCountryCode, Reference, UnstructuredMessage FROM dbo.TestData`

Select tabular report type.

![](Screens/Select%20Report%20Data.png)

Move the all fields into the Details text box.

Click the finish button.

Select Report Properties from the Report field in Extensions menu.

Click on the References tab followed by the add button then the browse tab and browse to the SwissQRBillGenerator.dll assembly in the GAC.

![](Screens/Add%20Reference.png)

Click on the Code tab and add the following function to the custom code text box:

```
Public Function QRcodeGeneration(ByVal account As String, ByVal creditorName As String, ByVal creditorAddressLine1 As String, ByVal creditorAddressLine2 As String, ByVal creditorCountryCode As String, ByVal amount As Decimal, ByVal currency As String, ByVal debtorName As String, ByVal debtorAddressLine1 As String, ByVal debtorAddressLine2 As String, ByVal debtorCountryCode As String, ByVal reference As String, ByVal unstructuredMessage As String) as Byte()
     Return SwissQRBillGenerator.SwissQRBillGeneratorClass.generateQRcode(account, creditorName, creditorAddressLine1, creditorAddressLine2, creditorCountryCode, amount, currency, debtorName, debtorAddressLine1, debtorAddressLine2, debtorCountryCode, reference, unstructuredMessage)
End Function
```

![](Screens/Add%20Custom%20Code.png)

Right-click on the top of the Unstructured Message column and insert a new column to the right.

Increase the height of the data row and the width of the new column to make room for a larger QRCode symbol.

Drag-and-drop an image component into the new field to bring up the image properties window.

![](Screens/Add%20ImageView.png)

Select Database as the image source and click on the use this field function button.

![](Screens/Select%20Data%20Base.png)

Enter the following code into the expression text box:

```
=Code.QRcodeGeneration(Fields!Account.Value, Fields!CreditorName.Value, Fields!CreditorAddressLine1.Value, Fields!CreditorAddressLine2.Value, Fields!CreditorCountryCode.Value, Fields!Amount.Value, Fields!Currency.Value, Fields!DebtorName.Value, Fields!DebtorAddressLine1.Value, Fields!DebtorAddressLine2.Value, Fields!DebtorCountryCode.Value, Fields!Reference.Value, Fields!UnstructuredMessage.Value)
```

![](Screens/Add%20Code%20to%20Expression.png)

Select image/png as MIME type:

![](Screens/Set%20Image%20Type.png)

Click on the size node and select the fit proportional radio button and click OK.

![](Screens/Select%20Image%20Size.png)

Click on the Preview tab of the report console.

The Bill data and Swiss QR code for the combined bill data are displayed in the report.