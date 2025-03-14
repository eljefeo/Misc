package org.example;

/**
 * Hello world!
 */
public class CalculatePi {
    public static void main(String[] args) {
        doPi();
    }

    private static void doPi() {
        //pi/4 = 1 - 1/3 + 1/5 - 1/7 + 1/9...

        String thousandDigitsOfPi = "3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679821480865132823066470938446095505822317253594081284811174502841027019385211055596446229489549303819644288109756659334461284756482337867831652712019091456485669234603486104543266482133936072602491412737245870066063155881748815209209628292540917153643678925903600113305305488204665213841469519415116094330572703657595919530921861173819326117931051185480744623799627495673518857527248912279381830119491298336733624406566430860213949463952247371907021798609437027705392171762931767523846748184676694051320005681271452635608277857713427577896091736371787214684409012249534301465495853710507922796892589235420199561121290219608640344181598136297747713099605187072113499999983729780499510597317328160963185950244594553469083026425223082533446850352619311881710100031378387528865875332083814206171776691473035982534904287554687311595628638823537875937519577818577805321712268066130019278766111959092164201989";

        double pi = getPi1();
        System.out.println("Pi: " + pi);
        checkCorrectDigitCount(thousandDigitsOfPi, String.valueOf(pi));


        double pi2 = getPi2();
        System.out.println("Pi2: " + pi2);
        checkCorrectDigitCount(thousandDigitsOfPi, String.valueOf(pi2));
    }

    private static double getPi1() {
        int max2 = 50000000; //1415926735902504 = found after max2 = 50000000
        int odd = 3;
        double sum = 1;
        boolean sign = false;

        //this second method will do twice as many calculations for max2 vs max1 below. Same results just need to make max1 below twice as large, I guess use this one
        for(int i = 0; i < max2; i++){
            sum += ((double) 1 / odd) * (sign ? 1 : -1);
            sign = !sign;
            odd += 2;
        }

        return 4 * sum;
    }

    private static double getPi2() {
        int max1 = 100000002;//1415926735902504 = found after max1 = 100000002
        double sum = 1;
        boolean sign = false;

        for (int i = 3; i < max1; i += 2) {
            //if max1 = 10, this loop will increase by 2 every time, so it will only do 5 calculations fyi (yes I know it starts at 3 so it will only do 4 calculations, you get what Im saying..)
            sum += ((double) 1 / i) * (sign ? 1 : -1);
            sign = !sign;
        }

        return 4 * sum;
    }

    public static void checkCorrectDigitCount(String actual, String mine) {
        if (mine.length() > actual.length()) {
            System.out.println("!! MAYBE ERROR ! ACTUAL LENGTH IS SHORTER THAN OURS");
        }
        int finalDigitPlace = 0;
        for (int i = 0; i < mine.length(); i++) {
            if (mine.charAt(i) != actual.charAt(i)) {
                finalDigitPlace = i - 2;
                System.out.println(finalDigitPlace + " Correct digits!");
                break;
            }
        }
        String sub = mine.substring(0, finalDigitPlace + 2);
        System.out.println("we had the first " + finalDigitPlace + " correct: " + sub);
    }
}
