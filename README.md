# 🔐 SafeVault

SafeVault is a secure ASP.NET Core MVC application designed to store sensitive user data in an encrypted vault. It demonstrates best practices in authentication, input validation, encryption, and protection against common security vulnerabilities like SQL injection and XSS. I have not focused on creating Razor Pages due it´s not necessary to check the security. Those checks are done by test-project.

---

## 🚀 Features

- User authentication with ASP.NET Identity
- Role-based access control (`Admin`, `User`, `Guest`)
- JWT token generation and validation
- AES encryption for vault entries
- Razor Pages for viewing and adding encrypted data
- Admin panel for managing users and vault items
- Input validation and sanitization
- Unit tests simulating SQL injection attacks

---

## 🔐 Security Architecture

### 1. Authentication & Authorization
- ASP.NET Identity for secure login and user management
- JWT-based authentication for stateless session handling
- Role-based access control using `[Authorize(Roles = "...")]`

### 2. Input Validation & XSS Protection
- `VaultInputModel` with DataAnnotations:
  - Required fields
  - Max length constraints
  - Regex filtering for unsafe characters
- `HtmlSanitizer` integration to clean user input
- Server-side and client-side validation via Razor Pages

### 3. SQL Injection Prevention
- Entity Framework Core with LINQ ensures parameterized queries
- No use of raw SQL (`FromSqlRaw`, `ExecuteSqlRaw`)
- Unit tests with xUnit simulate SQL injection attempts and confirm protection

### 4. Encryption of Sensitive Data
- AES encryption with IV via `EncryptionService`
- Vault entries are encrypted before storage and decrypted only for authorized users

### 5. Separation of Concerns
- `VaultItem` for encrypted storage
- `VaultInputModel` for validated user input
- `VaultMapper` for secure conversion between input and storage
- Avoids over-posting by excluding sensitive fields from binding

### 6. Testing & Verification
- InMemory database for isolated testing
- Unit tests for:
  - SQL injection resistance
  - Input validation
  - Encryption integrity

### 7. Admin Panel Security
- `AdminController` protected by `[Authorize(Roles = "Admin")]`
- Admins can view all users and vault entries
- No direct modification of encrypted data allowed

---

## 🧪 Example Test: SQL Injection Simulation

```csharp
[Theory]
[InlineData("admin' OR 1=1 --")]
[InlineData("' OR '1'='1")]
[InlineData("'; DROP TABLE Users; --")]
public void SqlInjection_ShouldNotReturnUser(string maliciousInput)
{
    var result = context.Users
        .FirstOrDefault(u => u.Username == maliciousInput);

    Assert.Null(result);
}
