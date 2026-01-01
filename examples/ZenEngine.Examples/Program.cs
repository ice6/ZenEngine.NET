using System.Text.Json;
using ZenEngine.Core;
using ZenEngine.Core.Loaders;
using ZenEngine.Core.Models;

namespace ZenEngine.Examples;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 ZenEngine.NET Examples");
        Console.WriteLine("========================\n");

        await RunSimpleDecisionExample();
        await RunPricingDecisionExample();
        await RunCompanyAnalysisExample();
        await RunDynamicPricingExample();
        await RunRealTimeQuotationExample();
        await RunMemoryLoaderExample();
        await RunTracingExample();

        Console.WriteLine("\n✅ All examples completed successfully!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static async Task RunSimpleDecisionExample()
    {
        Console.WriteLine("📝 Example 1: Simple Mathematical Decision");
        Console.WriteLine("------------------------------------------");

        try
        {
            var engine = new DecisionEngine(new FilesystemLoader("./decisions"));
            var result = await engine.EvaluateAsync("simple-decision.json", new { input = 15 });

            Console.WriteLine($"Input: 15");
            Console.WriteLine($"Result: {JsonSerializer.Serialize(result.Result, new JsonSerializerOptions { WriteIndented = true })}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task RunPricingDecisionExample()
    {
        Console.WriteLine("💰 Example 2: Customer Pricing Decision");
        Console.WriteLine("---------------------------------------");

        try
        {
            var engine = new DecisionEngine(new FilesystemLoader("./decisions"));
            
            // Test different customer scenarios
            var scenarios = new[]
            {
                new { 
                    name = "Premium Loyal Customer",
                    customer = new { age = 35, isPremium = true, loyaltyYears = 5 }
                },
                new { 
                    name = "Young Customer", 
                    customer = new { age = 16, isPremium = false, loyaltyYears = 0 }
                },
                new { 
                    name = "Standard Customer",
                    customer = new { age = 28, isPremium = false, loyaltyYears = 1 }
                }
            };

            foreach (var scenario in scenarios)
            {
                var result = await engine.EvaluateAsync("pricing-decision.json", scenario);
                Console.WriteLine($"Scenario: {scenario.name}");
                Console.WriteLine($"Result: {JsonSerializer.Serialize(result.Result, new JsonSerializerOptions { WriteIndented = true })}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task RunCompanyAnalysisExample()
    {
        Console.WriteLine("🏢 Example 3: Company Analysis");
        Console.WriteLine("------------------------------");

        try
        {
            var engine = new DecisionEngine(new FilesystemLoader("./decisions"));
            var context = new
            {
                country = "US",
                dateInc = "2014-12-31T16:00:00.000Z",
                industryType = "HC",
                annualRevenue = 1_500_000,
                creditRating = 770,
                companySize = "medium"
            };

            var result = await engine.EvaluateAsync("1.company-analysis.json", context);

            Console.WriteLine("Context:");
            Console.WriteLine(JsonSerializer.Serialize(context, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("Result:");
            Console.WriteLine(JsonSerializer.Serialize(result.Result, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task RunDynamicPricingExample()
    {
        Console.WriteLine("📈 Example 4: Dynamic Pricing");
        Console.WriteLine("-----------------------------");

        try
        {
            var engine = new DecisionEngine(new FilesystemLoader("./decisions"));
            var context = new
            {
                pricing = new
                {
                    basePrice = 100,
                    demand = "high",
                    timeOfDay = "normal",
                    competitorPrice = "equal",
                    customerSegment = "regular"
                }
            };

            var result = await engine.EvaluateAsync("2.dynamic-pricing.json", context);

            Console.WriteLine("Context:");
            Console.WriteLine(JsonSerializer.Serialize(context, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("Result:");
            Console.WriteLine(JsonSerializer.Serialize(result.Result, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task RunRealTimeQuotationExample()
    {
        Console.WriteLine("🧾 Example 5: Real-Time Quotation");
        Console.WriteLine("--------------------------------");

        try
        {
            var engine = new DecisionEngine(new FilesystemLoader("./decisions"));
            var context = new
            {
                generalLiability = 5_000_000,
                commercialProperty = 1_000_000,
                professionalIndemnity = 1_000_000
            };

            var result = await engine.EvaluateAsync("3.real-time-quotation.json", context);

            Console.WriteLine("Context:");
            Console.WriteLine(JsonSerializer.Serialize(context, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("Result:");
            Console.WriteLine(JsonSerializer.Serialize(result.Result, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("Expected:");
            Console.WriteLine(JsonSerializer.Serialize(new
            {
                paymentFee = "300.3",
                premium = "7800",
                tax = "780",
                total = "8880.3"
            }, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task RunMemoryLoaderExample()
    {
        Console.WriteLine("💾 Example 6: Memory Loader");
        Console.WriteLine("---------------------------");

        try
        {
            // Create a simple decision in memory
            var decisionContent = new DecisionContent
            {
                Id = "memory-decision",
                Name = "In-Memory Decision",
                Nodes = new Dictionary<string, Node>
                {
                    ["input1"] = new() { 
                        Id = "input1", 
                        Name = "Input", 
                        Type = "inputNode" 
                    },
                    ["expr1"] = new() { 
                        Id = "expr1", 
                        Name = "Square Calculator", 
                        Type = "expressionNode",
                        Content = new ExpressionNode 
                        {
                            Expressions = new Dictionary<string, string>
                            {
                                ["squared"] = "input * input",
                                ["cubed"] = "input * input * input"
                            }
                        }
                    },
                    ["output1"] = new() { 
                        Id = "output1", 
                        Name = "Output", 
                        Type = "outputNode" 
                    }
                },
                Edges = new List<Edge>
                {
                    new() { Id = "edge1", SourceId = "input1", TargetId = "expr1" },
                    new() { Id = "edge2", SourceId = "expr1", TargetId = "output1" }
                }
            };

            var memoryLoader = new MemoryLoader();
            memoryLoader.Add("square-decision", decisionContent);

            var engine = new DecisionEngine(memoryLoader);
            var result = await engine.EvaluateAsync("square-decision", new { input = 7 });

            Console.WriteLine($"Input: 7");
            Console.WriteLine($"Result: {JsonSerializer.Serialize(result.Result, new JsonSerializerOptions { WriteIndented = true })}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task RunTracingExample()
    {
        Console.WriteLine("🔍 Example 7: Execution Tracing and Performance");
        Console.WriteLine("-----------------------------------------------");

        try
        {
            var engine = new DecisionEngine(new FilesystemLoader("./decisions"));
            
            var options = new EvaluationOptions
            {
                IncludeTrace = true,
                IncludePerformance = true,
                MaxExecutionTimeMs = 5000
            };

            var result = await engine.EvaluateAsync("pricing-decision.json", 
                new { customer = new { age = 30, isPremium = true, loyaltyYears = 3 } }, 
                options);

            Console.WriteLine("📊 Execution Trace:");
            Console.WriteLine("==================");
            if (result.Trace != null)
            {
                foreach (var trace in result.Trace)
                {
                    Console.WriteLine($"Node: {trace.Name} ({trace.Type})");
                    Console.WriteLine($"  Execution Time: {trace.ExecutionTime:F2}ms");
                    Console.WriteLine($"  Input: {JsonSerializer.Serialize(trace.Input)}");
                    Console.WriteLine($"  Output: {JsonSerializer.Serialize(trace.Output)}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("⚡ Performance Metrics:");
            Console.WriteLine("=====================");
            if (result.Performance != null)
            {
                foreach (var metric in result.Performance)
                {
                    Console.WriteLine($"{metric.Key}: {metric.Value}");
                }
            }

            Console.WriteLine($"\n🎯 Final Result:");
            Console.WriteLine($"{JsonSerializer.Serialize(result.Result, new JsonSerializerOptions { WriteIndented = true })}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }
}
