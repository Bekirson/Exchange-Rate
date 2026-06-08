using System;
using System.Data.Common;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ExcelDataProccessor
{
    class Program
    {

        public class Transaction
        {
            public string? Id { get; set; }
            public string? ItemName { get; set; }
            public int? Quantity { get; set; }
            public float? Price { get; set; }
        }

        static void Main(string[] args)
        {
            string filePath = @"C:\Users\Bekirson\Desktop\C#\data\sales_data.csv";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File doesnt exist");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            List<string> transactions = new List<string>();

            AddTransaction(CreateTransaction("1","test", 1, 1), transactions);

            foreach (string x in transactions)
            {
                Console.WriteLine(x);
            }

            float totalRevenue = 0;
            int totalItemsSold = 0;
            int corruptedRowsCount = 0;
            int totalProcessed = 0;

            List<string> corruptedID = new List<string>();

            for (int i = 1; i < lines.Length; i++)
            {
                string currentLine = lines[i];

                float price = 0;
                int quantity = 1;

                string[] columns = currentLine.Split(',');

                if (columns.Length < 4)
                {
                    corruptedRowsCount++;
                    corruptedID.Add(columns[0]);
                    continue;
                }

                if (float.TryParse(columns[2], out price) && int.TryParse(columns[3], out quantity))
                {
                    totalItemsSold += quantity;
                    totalRevenue += price * quantity;
                }
                else
                {
                    corruptedRowsCount++;
                    corruptedID.Add(columns[0]);
                }

                totalProcessed++;
            }

            Console.WriteLine("--------- PROCESSING REPORT ---------");
            Console.WriteLine($"Total Revenue: ${totalRevenue}");
            Console.WriteLine($"Total Items Sold: {totalItemsSold}");
            Console.WriteLine($"Corrupted Rows Skipped: {corruptedRowsCount}");
            Console.WriteLine($"Total Amount Of Processes: {totalProcessed}");
            Console.WriteLine("-------------------------------------");

            foreach (String id in corruptedID)
            {
                Console.WriteLine($"Corrupted Rows IDs: {id}");
            }

            GenerateReport(totalRevenue, totalItemsSold, corruptedRowsCount, totalProcessed, corruptedID);
        }

        static public void GenerateReport(float totalRevenue, int totalItemsSold, int corruptedRowsCount, int totalProcessed, List<string> corruptedID)
        {
            string reportPath = @"C:\Users\Bekirson\Desktop\C#\data\summary_report.txt";

            StringBuilder reportBuilder = new StringBuilder();

            reportBuilder.AppendLine("--------- FINAL BUSINESS REPORT ---------");
            reportBuilder.AppendLine($"Processed On: {DateTime.Now}");
            reportBuilder.AppendLine($"Total Revenue Generated: ${totalRevenue:F2}");
            reportBuilder.AppendLine($"Total Physical Items Sold: {totalItemsSold}");
            reportBuilder.AppendLine($"Number of Corrupted Orders Flagged: {corruptedRowsCount}");
            reportBuilder.AppendLine($"Total Amount Of Processes: {totalProcessed}");
            reportBuilder.AppendLine("-----------------------------------------");
            reportBuilder.AppendLine("List of Flagged Corrupted Transaction IDs:");

            foreach (string id in corruptedID)
            {
                reportBuilder.AppendLine($"- {id}");
            }

            File.WriteAllText(reportPath, reportBuilder.ToString());

            Console.WriteLine($"Success! Report successfully compiled and saved to : {reportPath}");
            Console.ReadLine();
        }

        static public void AddTransaction(Transaction transaction, List<string> list)
        {
            if (transaction == null) return;

            list.Add($"{transaction.Id},{transaction.ItemName},{transaction.Price},{transaction.Quantity}");
        }

        static public Transaction CreateTransaction(string id, string item_name, float price, int quantity)
        {
            Transaction transaction = new Transaction
            {
                Id = id,
                ItemName = item_name,
                Price = price,
                Quantity = quantity
            };

            return transaction;
        }
    }
}
