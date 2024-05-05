package org.MontyHall;

import java.text.DecimalFormat;
import java.util.HashSet;
import java.util.Random;
import java.util.Set;


/*
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
::::::::::::::::::: Here is some output with different settings :::::::::::::::::::
::::: Each test below was run with 1,000,000 games played (one million games) :::::
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

-----------------------------------------------------------

With 3 doors and decided to switch doors when asked:
	--- I won 666,477 out of 1,000,000 games
	--- Winning Percentage: 66.65%

With 3 doors and decided *not* to switch doors when asked:
	--- I won 333,815 out of 1,000,000 games
	--- Winning Percentage: 33.38%

-----------------------------------------------------------

With 4 doors and decided to switch doors when asked:
	--- I won 750,104 out of 1,000,000 games
	--- Winning Percentage: 75.01%

With 4 doors and decided *not* to switch doors when asked:
	--- I won 250,059 out of 1,000,000 games
	--- Winning Percentage: 25.01%

-----------------------------------------------------------

With 10 doors and decided to switch doors when asked:
	--- I won 900,215 out of 1,000,000 games
	--- Winning Percentage: 90.02%

With 10 doors and decided *not* to switch doors when asked:
	--- I won 100,064 out of 1,000,000 games
	--- Winning Percentage: 10.01%

-----------------------------------------------------------

With 100 doors and decided to switch doors when asked:
	--- I won 989,873 out of 1,000,000 games
	--- Winning Percentage: 98.99%

With 100 doors and decided *not* to switch doors when asked:
	--- I won 10,053 out of 1,000,000 games
	--- Winning Percentage: 1.01%

-----------------------------------------------------------


 */

public class MontyHallGame {

    private static final Random rand = new Random(); //This will help us to get random numbers
    private static final DecimalFormat formatter = new DecimalFormat("#,###"); //used to format numbers when logged on screen

    public static void main(String[] args) {

        int numberOfDoors = 100;
        int howManyTimeToRun = 1000000;
        boolean shouldSwitchDoors = false;
        boolean shouldShowText = false;
        testMontyHallLotsOfTimes(howManyTimeToRun, shouldSwitchDoors, shouldShowText, numberOfDoors);
    }

   public static void testMontyHallLotsOfTimes(int howMany, boolean shouldSwitchMyDoor, boolean shouldShowText, int numberOfDoors){
        int winCount = 0;
        for (int i = 0; i < howMany; i++)
            if(montyHallManyDoors(shouldSwitchMyDoor, shouldShowText, numberOfDoors))
                winCount++;
        double winPercentage = ((double)winCount / (double)howMany) * 100;
        System.out.println("\t--- I won " + formatter.format(winCount) + " out of " + formatter.format(howMany) + " games");
        System.out.println("\t--- Winning Percentage: " + new DecimalFormat("#.##").format(winPercentage) + "%\n");
    }




    public static boolean montyHallManyDoors(boolean shouldSwitchMyDoor, boolean shouldShowText, int howManyDoors){
        if(howManyDoors < 3){
            System.out.println("Error - Please pick a number of doors greater than 2... Choose 3 or more.");
            return false;
        }

        int prizeDoor = pickRandomDoor(howManyDoors);
        int myDoor = pickRandomDoor(howManyDoors);
        int otherDoorToKeepShut = prizeDoor;

        if(prizeDoor == myDoor){
            while(otherDoorToKeepShut == prizeDoor){
                otherDoorToKeepShut = pickRandomDoor(howManyDoors); //since the person actually picked the winning door right off the bat,
                                                                    // pick a random other door to keep shut, so we can ask them if they want to switch

                if(shouldShowText)// Just to show some info on screen, doesn't actually change anything
                    System.out.println("Picked prize door, choosing this random door to keep shut: " + otherDoorToKeepShut);
            }
        }

        Set<Integer> doorsToTakeAway = new HashSet<>();
        for(int i = 0; i < howManyDoors; i++){
            if(i != myDoor && i != prizeDoor && i != otherDoorToKeepShut){
                doorsToTakeAway.add(i); // Open all the other doors besides the one the person picked and one other door
            }
        }

        if(shouldShowText) { // Just to show some info on screen, doesn't actually change anything
            System.out.println("doors at first are:");
            System.out.println("Prize Door = " + prizeDoor);
            System.out.println("My Door = " + myDoor);
            System.out.println("Other door to keep shut = " + otherDoorToKeepShut);
            for(Integer i : doorsToTakeAway){
                System.out.println("Door to take away: " + i);
            }
        }
        if (shouldSwitchMyDoor){
            if(shouldShowText)// Just to show some info on screen, doesn't actually change anything
                System.out.println("Switching doors from " + myDoor + " to " + otherDoorToKeepShut);

            myDoor = otherDoorToKeepShut; //switch your door for the only other one left closed
        }

        return myDoor == prizeDoor; //result is whether your door is equal to the winning door
    }

    private static int pickRandomDoor(int numberOfDoors){
        return rand.nextInt(numberOfDoors);
    }

    public static boolean montyHallStandard(boolean shouldSwitchMyDoor, boolean shouldShowText){
        //This will just do the ol fashioned Monty Hall game with just 3 doors

        boolean iWon = montyHallManyDoors(shouldSwitchMyDoor, shouldShowText, 3);
        if(iWon){
            System.out.println("You picked the right door!!! Congrats you win!!!");
        } else {
            System.out.println("Oh nooOOoozz you did not pick the prize door, you get a smelly goat to take home ... ): ");
        }
        return iWon;
    }

}
