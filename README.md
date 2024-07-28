---

# Soundsystem ASP.NET Core MVC Project

## Overview

This project is an e-commerce platform for music-related products. It includes various features like recently viewed products, filtering options, blog comments, social media sharing, email operations, OTP verification, product creation, and an admin panel for website modifications.

## Table of Contents

1. [Technologies Used](#technologies-used)
2. [Features](#features)
3. [Pages and Functionalities](#pages-and-functionalities)
4. [Setup and Installation](#setup-and-installation)
5. [Database Configuration](#database-configuration)
6. [SMTP and OTP Configuration](#smtp-and-otp-configuration)
7. [Admin Page](#admin-page)
8. [Contribution Guidelines](#contribution-guidelines)
9. [License](#license)

## Technologies Used

- **ASP.NET Core MVC**: For building the web application.
- **SQL Server**: As the database server.
- **SMTP**: For email operations.
- **jQuery**: For dynamic content manipulation.
- **CSS/HTML**: For styling and structuring the web pages.

## Features

- **Recently Viewed Products**: Display products recently viewed by the user on the home page.
- **Shop Filters**: Filter products by various criteria on the shop page.
- **Blog Comments**: Users can add and remove comments on blog posts.
- **Social Media Sharing**: Share blog posts on LinkedIn and Facebook.
- **Email Operations**: Includes forget password and reset password functionalities via SMTP.
- **OTP Verification**: Secure operations with One-Time Password (OTP) verification.
- **Product Creation**: Users can create products, which are then checked by an expert before being published.
- **Admin Page**: For modifying website content and managing products.

## Pages and Functionalities

### Home Page

- **Recently Viewed Products**: Displays products that the user has recently viewed.

### Shop Page

- **Product Filters**: Allows users to filter products based on categories, price, ratings, etc.

### Blog Page

- **Comments**: Users can add and remove comments on blog posts.
- **Social Media Sharing**: Share blogs on LinkedIn and Facebook.

### Login/Register Page

- **Forget Password**: Users can request a password reset.
- **Reset Password**: Users can reset their password via a link sent to their email.
- **OTP Verification**: Ensures secure login and registration processes.

### Contact Page

- **Contact Form**: Users can fill out a form to contact support.

### My Product Page

- **Create Product**: Users can create new products.
- **Expert Review**: Created products are checked by an expert before being published.

### Admin Page

- **Modify Website**: Admins can modify website content and manage products.

## Setup and Installation

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/yourusername/soundsystem-aspnet-core-mvc.git
    cd soundsystem-aspnet-core-mvc
    ```

2. **Configure the Database**:
    Update the connection string in `appsettings.json` to point to your SQL Server instance.

3. **Run Migrations**:
    ```bash
    dotnet ef database update
    ```

4. **Configure SMTP and OTP**:
    Update the SMTP settings in `appsettings.json`.

5. **Run the Application**:
    ```bash
    dotnet run
    ```

## Database Configuration

Ensure that your `appsettings.json` contains the correct connection string for your SQL Server. Example:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
}
```

## SMTP and OTP Configuration

Update the SMTP settings in `appsettings.json` for email operations:
```json
"EmailSettings": {
    "SMTPServer": "smtp.your-email-provider.com",
    "Port": 587,
    "SenderName": "Your Name",
    "SenderEmail": "your-email@example.com",
    "Username": "your-smtp-username",
    "Password": "your-smtp-password"
},
"OTPSettings": {
    "OTPKey": "your-otp-key",
    "ExpiryInMinutes": 5
}
```

## Admin Page

The admin page allows for managing website content and products. Ensure you have the appropriate access rights configured.

## Contribution Guidelines

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -am 'Add some feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Create a new Pull Request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---
