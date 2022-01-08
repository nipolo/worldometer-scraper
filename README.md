### How to run
1. Clone the repo in local folder
1. Install Chrome and check the chrome version
1. Download chromedriver for the Chrome version from [here](https://chromedriver.chromium.org/downloads) and copy to solution_folder/ConsoleApp/chrome
1. Create the DB and update with latest migrations
    ```console 
    cd solution_folder/Data
    dotnet ef database update
    ```
1. Change ConnectionString for DefaultConnection in appsettings.json that corresponds to the right location of the already created db file
1. Run the console app with parameter
    ```console 
    cd solution_folder/ConsoleApp
    dotnet run --region All
    dotnet run --region NorthAmerica
    ```
1. Check in the DB that the records are saved