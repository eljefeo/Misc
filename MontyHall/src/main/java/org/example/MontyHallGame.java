package org.example;

import java.text.DecimalFormat;
import java.util.HashSet;
import java.util.Random;
import java.util.Set;

public class MontyHallGame {

    private static Random rand;


    public static void main(String[] args) {

        int numberOfDoors = 3;
        int howManyTimeToRun = 1000;
        boolean shouldSwitchDoors = true;
        boolean shouldShowText = false;
        testMontyHallLotsOfTimes(howManyTimeToRun, shouldSwitchDoors, shouldShowText, numberOfDoors);
    }

   public static void testMontyHallLotsOfTimes(int howMany, boolean shouldSwitchMyDoor, boolean shouldShowText, int numberOfDoors){
        int winCount = 0;
        for (int i = 0; i < howMany; i++)
            if(montyHallManyDoors(shouldSwitchMyDoor, shouldShowText, numberOfDoors))
                winCount++;
        double winPercentage = ((double)winCount / (double)howMany) * 100;
        System.out.println("I won " + winCount + " out of " + howMany);
        System.out.println("Winning Percentage: " + new DecimalFormat("#.##").format(winPercentage) + "%\n");
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

    public static boolean montyHallManyDoors(boolean shouldSwitchMyDoor, boolean shouldShowText, int howManyDoors){
        if(howManyDoors < 3){
            System.out.println("Error - Please pick a number of doors greater than 2... Choose 3 or more.");
            return false;
        } else if(rand == null){
            rand = new Random();
        }

        int prizeDoor = pickRandomDoor(howManyDoors);
        int myDoor = pickRandomDoor(howManyDoors);
        int otherDoorToKeepShut = prizeDoor;

        if(prizeDoor == myDoor){
            while(otherDoorToKeepShut == prizeDoor){
                otherDoorToKeepShut = pickRandomDoor(howManyDoors); //since the person actually picked the winning door right off the bat,
                                                                    // pick a random other door to keep shut so we can ask them if they want to switch
                if(shouldShowText)
                    System.out.println("Picked prize door, choosing this random door to keep shut: " + otherDoorToKeepShut);
            }
        }

        Set<Integer> doorsToTakeAway = new HashSet<>();
        for(int i = 0; i < howManyDoors; i++){
            if(i != myDoor && i != prizeDoor && i != otherDoorToKeepShut){
                doorsToTakeAway.add(i);
            }
        }


        if(shouldShowText) {
            System.out.println("doors at first are:");
            System.out.println("Prize Door = " + prizeDoor);
            System.out.println("My Door = " + myDoor);
            System.out.println("Other door to keep shut = " + otherDoorToKeepShut);
            for(Integer i : doorsToTakeAway){
                System.out.println("Door to take away: " + i);
            }
        }
        if (shouldSwitchMyDoor){
            if(shouldShowText)
                System.out.println("Switching doors from " + myDoor + " to " + otherDoorToKeepShut);

            myDoor = otherDoorToKeepShut;
        }

        return myDoor == prizeDoor;
    }

    private static int pickRandomDoor(int numberOfDoors){
        return rand.nextInt(numberOfDoors);
    }

}
