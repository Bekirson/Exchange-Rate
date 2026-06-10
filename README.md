# Live Exchange Rate Consumer & Converter

A standalone C# console application designed to consume live financial market data from a public REST API. The application processes live exchange rates to perform real-time currency conversions between US Dollars (USD) and Turkish Liras (TRY).

## 📡 Core System Capabilities
* **Live API Ingestion:** Utilizes non-blocking asynchronous requests to query production financial market endpoints.
* **JSON Serialization Pipeline:** Seamlessly parses structural HTTP string responses into type-safe C# data structures.
* **Instant Currency Conversion:** Performs algorithmic valuation calculation updates to convert financial data values between currency nodes instantly.

## 🏗️ Architecture Flow
```text
[Public Financial API] ──(JSON String)──► [HttpClient Consumer] ──► [Deserialization Engine] ──► [Console Application UI]