# Take-home Test Backend

This project is a .NET 8 backend application that provides REST APIs for managing invoices. The application is dockerized for easy deployment.

## REST APIs

### Get Invoice by Invoice Number

- **Endpoint**: `GET /api/invoices/{invoiceNumber}`
- **Description**: Retrieves an invoice by its invoice number.
- **Response**:
  - `200 OK`: Returns the invoice details.
  - `404 Not Found`: If the invoice does not exist.

### Get Invoices

- **Endpoint**: `GET /api/invoices`
- **Description**: Retrieves all invoices or filters invoices by client name.
- **Query Parameters**:
  - `name` (optional): The name of the client to filter invoices.
- **Response**:
  - `200 OK`: Returns a list of invoices.
  - `404 Not Found`: If no matching invoices are found.

### Add Invoice

- **Endpoint**: `POST /api/invoices`
- **Description**: Adds a new invoice.
- **Request Body**: 
  { "InvoiceNumber": "string", "ClientId": "string", "DueDate": "DateTime", "Status": "string", "LineItems": [ { "Description": "string", "Quantity": "int", "Price": "decimal" } ] }
- **Response**:
    - `201 Created`: Returns the created invoice.
    - `400 Bad Request`: If the request body is invalid.

## Running the Application with Docker

### Prerequisites

- Docker installed on your machine.

### Building the Docker Image

1. Navigate to the directory containing the `Dockerfile`.

2. Build the Docker image:
   docker build -t take-home-test-backend:latest -f Dockerfile .

### Running the Docker Container

1. Run the Docker container:
   docker run -d -p 5000:5000 --name take-home-test-backend take-home-test-backend:latest
