using System;
class Program{
    static void Main(){

        //

        // Console.WriteLine("HEllo world");
        // Console.WriteLine(sum(4,5));
        // evenodd(15);

        int []arr={4,5,9,7,5,6,3};
        arrsum(arr);
    }

    static int sum(int a, int b){
        return a+b;

    }
    static void evenodd(int n){
        for(int i=0; i<=n; i++){
            if(i%2==0){
                Console.WriteLine(i+" is even");
            }
            else{
                Console.WriteLine(i+" is odd");
            }
        }

    }

    static void arrsum(int []arr){
        int sum=0;
        for(int i=0; i<arr.Length;i++){
            sum+=arr[i];
        }
        Console.WriteLine("sum of array elements is: "+sum);
    }

    
        
}
