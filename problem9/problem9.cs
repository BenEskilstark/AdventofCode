public class Problem9 {
    public static void Solve() {
        long sum = 0;
        foreach (string line in File.ReadAllLines("problem9/testinput.txt")) {
            List<int> nums = line.Split(" ").Select(int.Parse).ToList();
            // Utils.PrintList(nums);
            List<List<int>> seq = GenSeq(nums);
            List<int> sol = GenSol2(seq);
            // sum += sol.Aggregate((a,b) => a + b);
            sum += sol[^1];
            // Utils.PrintList(sol);
            // Console.WriteLine("");
        }
        Console.WriteLine(sum);
    }

    public static List<int> GenSol2(List<List<int>> seq) {
        List<int> sol = [0];
        for (int i = 1; i <= seq.Count(); i++) {
            sol.Add(seq[seq.Count - i][0] - sol[i - 1]);
        }
        return sol;
    }

    public static List<int> GenSol(List<List<int>> seq) {
        List<int> sol = [0];
        for (int i = 1; i <= seq.Count; i++) {
            sol.Add(seq[seq.Count - i][^1] + sol[i - 1]);
        }
        return sol;
    }

    public static List<List<int>> GenSeq(List<int> nums) {
        List<List<int>> seq = [nums];

        while (!seq[^1].All(j => j == 0)) {
            List<int> nextNums = [];
            for (int i = 1; i < seq[^1].Count; i++) {
                nextNums.Add(seq[^1][i] - seq[^1][i-1]);
            }
            // Utils.PrintList(nextNums);
            seq.Add(nextNums);
        }
        
        return seq;
    }
}