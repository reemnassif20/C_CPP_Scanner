namespace C_C___Scanner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string simpleTestCode = @"
                int main() {
                    // This is a comment
                    float x = 3.14;
                    /* This is a
                       multiline comment */
                    if (x > 0) {
                        return 1;
                    }
                    return 0;
                }";

            string advancedTestCode = @"
                #include <iostream>
                using namespace std;

                class Example {
                public:
                    int x;
                    Example(int x) : x(x) {}
        
                    void show() {
                        cout << ""Value: "" << x << endl;
                    }
                };

                int main() {
                    Example example(10);
                    example.show();
        
                    // Single line comment
                    int i = 0;
                    double y = 2.5;
                    string str = ""Hello, World!"";
        
                    if (y > 1.0 && y < 10.0) {
                        cout << ""Condition met!"" << endl;
                    } else if (y == 2.5) {
                        cout << ""Exact match"" << endl;
                    } else {
                        cout << ""No match"" << endl;
                    }

                    // Loop and bitwise operators
                    for (i = 0; i < 5; i++) {
                        y *= i | 1;
                    }

                    // Switch case
                    switch (i) {
                        case 1:
                            cout << ""One"" << endl;
                            break;
                        case 2:
                            cout << ""Two"" << endl;
                            break;
                        default:
                            cout << ""Other"" << endl;
                    }
        
                    /* Multiline
                       comment */
                    return 0;
                }";

            Console.WriteLine("###########################  SIMPLE TEST CODE ###########################");
            Console.WriteLine(simpleTestCode);
            Scanner scanner = new Scanner(simpleTestCode);
            Token token;
            do
            {
                token = scanner.GetNextToken();
                Console.WriteLine(token);
            } while (token.Type != TokenType.EOF);

            Console.WriteLine("###########################  ADVANCED TEST CODE ###########################");
            Console.WriteLine(advancedTestCode);
            Scanner scanner2 = new Scanner(advancedTestCode);
            Token token2;
            do
            {
                token2 = scanner2.GetNextToken();
                Console.WriteLine(token2);
            } while (token2.Type != TokenType.EOF);
        }
    }
}
