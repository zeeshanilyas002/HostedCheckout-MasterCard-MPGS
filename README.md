### 📦 **Project Configuration Overview for Mastercard Hosted Checkout Integration**

This configuration file governs the setup for integrating with **Mastercard Payment Gateway Services (MPGS)**, specifically for **Hosted Checkout** functionality, with logging and security handled through **Serilog**, **AWS Secrets Manager**, and optional **CloudWatch**.

---

### 🔐 **Authentication**

#### `BasicAuth`

Used for basic API authentication within internal systems or management dashboards (e.g., Swagger UI or internal endpoints).

* **userName**: Username for basic authentication.
* **password**: Password for basic authentication.

---

### 📊 **Logging Configuration**

#### `Serilog`

Manages structured logging across the application:

* `MinimumLevel.Default`: Logs information-level messages by default.
* `Override`:

  * Suppresses detailed logs from `Microsoft`, `System`, and `HttpClient` to warning level to reduce noise.

#### `LoggingOptions`

Controls how logs are stored and where they are sent:

* `UseFile`: Enables file-based logging.
* `UseCloudWatch`: Enables or disables sending logs to AWS CloudWatch.
* `LogGroup`: CloudWatch log group name.
* `Region`: AWS region for CloudWatch logging.
* `AccessKey`, `SecretKey`: AWS credentials for logging access (should be stored securely, preferably using environment variables or secret manager).

---

### ☁️ **AWS Configuration**

#### `AWS`

Handles access to sensitive data such as Mastercard credentials via **AWS Secrets Manager**:

* `Region`: Region of the AWS Secret Manager where secrets are stored.
* `AccessKey`, `SecretKey`: AWS IAM credentials (restricted to read-only access to secrets).
* `SecretName`: The secret name in Secrets Manager that contains MPGS API credentials.

---

### 💳 **MPGS (Mastercard Payment Gateway Services) Integration**

#### `MPGS`

The section to configure your MPGS endpoint and credentials:

* `apiBaseUrl`: Base URL of the MPGS API (e.g., `https://eu-gateway.mastercard.com/api/nvp/version`).
* `merchantId`: Your registered MPGS merchant identifier.
* `APIKey`: API key provided by Mastercard to authenticate requests.
* `apiVersion`: The version of the API you’re integrating with (e.g., `62`).

> 🔒 **Note**: It is highly recommended to fetch sensitive MPGS fields (merchantId, APIKey, etc.) from **AWS Secrets Manager**, especially for production environments.

---

### 🌐 **Allowed Hosts**

* `AllowedHosts`: Controls which hosts are permitted to access this application. `"*"` allows all (suitable for development, but should be tightened for production).


### **Test View for Hosted Checkout**

![image](https://github.com/user-attachments/assets/65f5ad36-5e97-4732-bf01-274575479347)

### **After clicking pay button it will be redirected to master card hosted page**

we used javascript library for hosted checkout
https://test-gateway.mastercard.com/static/checkout/checkout.min.js

![image](https://github.com/user-attachments/assets/3d19ac0b-75d2-4f02-a2a4-28eb3e094df6)





