# AFAS SB Integration Example

This project demonstrates how to integrate with AFAS SB using their APIs.
AFAS SB is a powerful software suite for businesses that includes functionalities such as financial administration.

## Overview

The example application supports various API operations including:

1. Authorization using PKCE (Proof Key for Code Exchange) to obtain an access token.
2. Retrieving a list of organizations, persons, payment conditions, administrations, and ledger accounts.
3. Sending a sales journal entry.

The project consists of two main classes: `AfasAuthClient` and `AfasApiClient`. The `AfasAuthClient` handles the authentication and authorization flow, while the `AfasApiClient` handles the actual API requests and responses.

## Prerequisites

Before you can run this project, you need to provide your AFAS instance details and credentials including client URL, client ID, client secret, and a redirect URI. Please contact your AFAS contact to get these details or use https://partner.afas.nl/aanmelden to get in touch.

Please read the [API Documentation](https://docs.afas.help/sb/nl/start)

## Getting Started

1. Clone the repository.

2. Open the solution in your preferred C# development environment, such as Visual Studio.

3. Update the `Program` class with your AFAS instance details and credentials.

4. Build and run the application.

When the application runs, it will first authenticate using PKCE, then retrieve some records like organizations and ledger accounts, and finally send a sales journal entry to your AFAS instance.

## Notes

This example application is designed for educational purposes to help developers understand how to integrate with AFAS SB API. Please customize it to meet your specific project requirements and ensure proper error handling, API usage throttling, and other best practices are implemented.
