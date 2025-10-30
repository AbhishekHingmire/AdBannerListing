# AdBannerListings - Deployment Guide for Somee Hosting

## Issue: Database Connection Error on Somee Hosting

The error you're seeing is likely because Somee free hosting doesn't allow connections to external databases like Azure SQL Database. I've implemented two solutions:

## Solution 1: Use Somee's SQL Server Database (Recommended)

### Steps:
1. Get your Somee database connection details from the control panel
2. Update `appsettings.Production.json` with your Somee database details
3. Create the Banner table manually in your Somee database

## Solution 2: Use SQLite (Easiest for Free Hosting)

The application now automatically uses SQLite in production mode, which works with most free hosting providers.

### For SQLite:
- No database setup required
- The database file will be created automatically
- Works with free hosting that allows file system access

### To use SQLite:
1. Publish the application as-is
2. The app will automatically create `banners.db` in the application directory
3. No additional configuration needed

## Configuration Files Updated:

### appsettings.Production.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SOMEE_SERVER.somee.com;Database=YOUR_DATABASE_NAME;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Program.cs Changes:
- Added detailed error pages for debugging
- Added database initialization
- Added conditional database provider (SQLite for production, SQL Server for development)
- Added comprehensive error handling and logging

## Manual Table Creation (if using Somee SQL Server):

```sql
CREATE TABLE [dbo].[Banners] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Title] NVARCHAR(200) NOT NULL,
    [ImageURL] NVARCHAR(MAX) NULL,
    [RedirectURL] NVARCHAR(MAX) NULL,
    [Latitude] FLOAT NULL,
    [Longitude] FLOAT NULL,
    [Radius] FLOAT NULL
);
```

## Security Note:
After debugging is complete, change the error handling back to production mode by updating `Program.cs`:

```csharp
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Remove UseDeveloperExceptionPage()
    app.UseHsts();
}
```

## Testing:
1. Publish the updated application
2. Visit your banner page
3. Check for detailed error messages
4. The app should now work with SQLite automatically, or show specific database connection errors if using external DB