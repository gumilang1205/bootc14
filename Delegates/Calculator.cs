public class Calculator
{
    private int multiplier;

    public Calculator(int multiplier)
    {
        this.multiplier = multiplier;
    }

    public int Multiplier => multiplier;

    // Instance method that matches our Transformer delegate
    public int MultiplyBy(int input)
    {
        return input * multiplier;
    }
}

// File processor for real-world scenario
public class FileProcessor
{
    // Event using multicast delegate
    public event Action<int>? Progress;

    // Method that uses strategy pattern with delegates
    public void ProcessFiles(string[] fileNames, Func<string, string> processingStrategy)
    {
        for (int i = 0; i < fileNames.Length; i++)
        {
            // Calculate progress
            int percent = (i * 100) / fileNames.Length;
            Progress?.Invoke(percent);  // Notify all subscribers

            // Apply the plugged-in processing strategy
            string result = processingStrategy(fileNames[i]);
            Console.WriteLine($"    Result: {result}");
        }

        // Final progress report
        Progress?.Invoke(100);
    }
}