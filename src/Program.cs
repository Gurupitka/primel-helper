using Trie.Net.Standard;

namespace PrimeSearch 
{
    public class Program 
    {
        public static void Main() 
        {
            var trie = new Trie<int>();
            using StreamReader streamReader = new StreamReader(File.Open(@"primes.txt", FileMode.Open));
            while(!streamReader.EndOfStream) {
                int prime = int.Parse(streamReader.ReadLine());
                trie.Insert(prime);
            }
          
            var search = "0";
            while(!string.IsNullOrEmpty(search)) {
                Console.WriteLine("search (**6** validvalues invalidvalues):");
                search = Console.ReadLine();
                var parameters = parseSearch(search);
                if(string.IsNullOrEmpty(search)) {
                    break;
                }
                var results = trie.Search(x =>  { 
                    var xAsString = x.Value.ToString();
                    var positionalPass = parameters.PositionalRequirements(xAsString);
                    var acceptPass = parameters.ContainsAcceptableValues != null ? parameters.ContainsAcceptableValues(xAsString) : true; 
                    var excludePass = parameters.DoesNotContainExcludedValues != null ? parameters.DoesNotContainExcludedValues(xAsString) : true;
                    return positionalPass && acceptPass && excludePass;
                 });
                Console.WriteLine("=======================");
                foreach(var result in results){
                    Console.WriteLine(result.Value);
                }
            }
        }

        public static SearchParams parseSearch(string search)
        {
            var result = new SearchParams();
            var components = search.Split(" ");
            Predicate<string> positionalRequirements;
            var positionAndValues = new List<Tuple<int,string>>();
            for(int i = 0; i < components[0].Length; i++) {
                //string could be * * 6 * * for example, meaning we should exclude strings which don't have 6 in position 2.
                var letter = components[0][i];
                if(letter != '*') {                    
                    positionAndValues.Add(new Tuple<int,string>(i,letter.ToString()));
                }
            }
            result.PositionalRequirements = (x => {
                foreach(var val in positionAndValues) {                    
                    if(x.Length < 5 || x[val.Item1].ToString() != val.Item2) {
                        return false;
                    }                    
                }
                return true;
            });

            var acceptableValues = new HashSet<string>();
            Predicate<string> containsAcceptableValues;
            if(components.Length > 1) {
                //include
                foreach(var val in components[1]) {
                    acceptableValues.Add(val.ToString());
                }
                result.ContainsAcceptableValues = (x => {
                    foreach(var l in x) {
                        if(!acceptableValues.Contains(l.ToString())){
                            return false;
                        }
                    }
                    return true;
                });
            }

            if(components.Length >2) {
                //exclude these values.
                Predicate<string> doesNotContainExcludedValues;
                var unacceptableValues = new HashSet<string>();
                foreach(var val in components[2]) {
                    unacceptableValues.Add(val.ToString());
                }

                result.DoesNotContainExcludedValues = (x => {
                    foreach(var l in x) {
                        if(unacceptableValues.Contains(l.ToString())){
                            return false;
                        }
                    }
                    return true;
                });
            }

            return result;
        }   
    }

    public class SearchParams 
    {
        public Predicate<string> PositionalRequirements {get;set;} 
        public Predicate<string> ContainsAcceptableValues {get;set;} 

        public Predicate<string> DoesNotContainExcludedValues {get;set;}
    }
}