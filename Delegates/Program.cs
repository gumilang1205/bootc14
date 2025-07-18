namespace Delegates
{
    class Program
    {
        static void Main(string[] args)
        {
            //BasicDelegateDemo();
            //PluginMethodsDemo();
            InstanceAndStaticMethodTargetsDemo();
        }
        delegate int Transformer(int x);

        static void BasicDelegateDemo()
        {
            Console.WriteLine("1. BASIC DELEGATE USAGE - THE FOUNDATION");
            Console.WriteLine("========================================");

            // Step 1: Create a delegate instance pointing to a method
            Transformer t = Square;  // This is shorthand for: new Transformer(Square)

            // Step 2: Invoke the delegate just like calling a method
            int result = t(3);  // This calls Square(3) through the delegate

            Console.WriteLine($"Square of 3 through delegate: {result}");

            // The beauty is indirection - we can change what method gets called
            t = Cube;  // Now t points to a different method
            result = t(3);  // Same syntax, different behavior

            Console.WriteLine($"Cube of 3 through same delegate: {result}");

            // You can also use the explicit Invoke method
            result = t.Invoke(4);
            Console.WriteLine($"Cube of 4 using Invoke: {result}");

            Console.WriteLine();
        }
        static int Square(int x) => x * x;
        static int Cube(int x) => x * x * x;

        static void PluginMethodsDemo()
        {
            Console.WriteLine("2. WRITING PLUGIN METHODS WITH DELEGATES");
            Console.WriteLine("========================================");

            // This demonstrates the power of delegates for creating pluggable behavior
            int[] values = { 1, 2, 3, 4, 5 };

            Console.WriteLine($"Original values: [{string.Join(", ", values)}]");

            // Transform array using Square as the plugin
            Transform(values, Square);
            Console.WriteLine($"After Square transform: [{string.Join(", ", values)}]");

            // Reset values
            values = new int[] { 1, 2, 3, 4, 5 };

            // Same Transform method, different behavior by passing different delegate
            Transform(values, Cube);
            Console.WriteLine($"After Cube transform: [{string.Join(", ", values)}]");

            // You can even pass lambda expressions as plugins
            values = new int[] { 1, 2, 3, 4, 5 };
            Transform(values, x => x + 10);  // Add 10 to each element
            Console.WriteLine($"After +10 transform: [{string.Join(", ", values)}]");

            Console.WriteLine();
        }
        static void Transform(int[] values, Transformer t)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = t(values[i]);  // Apply the plugged-in transformation
        }

        static void InstanceAndStaticMethodTargetsDemo()
        {
            Console.WriteLine("3. INSTANCE AND STATIC METHOD TARGETS");
            Console.WriteLine("=====================================");

            // Static method target - no object instance needed
            Console.WriteLine("Static method delegation:");
            Transformer staticDelegate = Square;
            Console.WriteLine($"Static Square of 6: {staticDelegate(6)}");

            // Instance method target - delegate holds both method AND object reference
            Console.WriteLine("\nInstance method delegation:");
            Calculator calc = new Calculator(5);  // Object with multiplier = 5
            Transformer instanceDelegate = calc.MultiplyBy;  // Points to instance method

            Console.WriteLine($"Multiply 8 by {calc.Multiplier}: {instanceDelegate(8)}");

            // The delegate keeps the object alive - demonstrate this with Target property
            Console.WriteLine($"Delegate Target is null (static): {staticDelegate.Target == null}");
            Console.WriteLine($"Delegate Target is Calculator instance: {instanceDelegate.Target is Calculator}");

            // Multiple instances, multiple delegates
            Calculator calc2 = new Calculator(3);
            Transformer instanceDelegate2 = calc2.MultiplyBy;

            Console.WriteLine($"Different instance - multiply 8 by {calc2.Multiplier}: {instanceDelegate2(8)}");

            Console.WriteLine();
        }

    }
}
