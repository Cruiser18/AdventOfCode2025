namespace Solution1;

public class BatteryBank
{
    public static int FindMaxJoltage(string bank)
    {
        int maxJoltage = 0;
        
        // Try all pairs of digits in order (i before j)
        for (int i = 0; i < bank.Length - 1; i++)
        {
            for (int j = i + 1; j < bank.Length; j++)
            {
                int tens = int.Parse(bank[i].ToString());
                int ones = int.Parse(bank[j].ToString());
                int joltage = tens * 10 + ones;
                
                if (joltage > maxJoltage)
                {
                    maxJoltage = joltage;
                }
            }
        }
        
        return maxJoltage;
    }
    
    public static int CalculateTotalJoltage(string[] banks)
    {
        int total = 0;
        foreach (var bank in banks)
        {
            total += FindMaxJoltage(bank);
        }
        return total;
    }
    
    public static long FindMaxJoltage12(string bank)
    {
        // We need to select exactly 12 digits that form the largest number
        // Strategy: Skip the smallest digits to keep only 12
        int digitsToKeep = 12;
        int digitsToSkip = bank.Length - digitsToKeep;
        
        var result = new System.Text.StringBuilder();
        int skipped = 0;
        
        for (int i = 0; i < bank.Length; i++)
        {
            char currentDigit = bank[i];
            
            // Remove smaller digits from result if we still have room to skip
            while (result.Length > 0 && 
                   result[result.Length - 1] < currentDigit && 
                   skipped < digitsToSkip)
            {
                result.Length--;
                skipped++;
            }
            
            result.Append(currentDigit);
        }
        
        // Trim to exactly 12 digits (remove from end if needed)
        while (result.Length > digitsToKeep)
        {
            result.Length--;
        }
        
        return long.Parse(result.ToString());
    }
    
    public static long CalculateTotalJoltage12(string[] banks)
    {
        long total = 0;
        foreach (var bank in banks)
        {
            total += FindMaxJoltage12(bank);
        }
        return total;
    }
}
