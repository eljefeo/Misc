package org.PasswordGenerator;

import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

/*
::::::::::::::::::::: Here is the output when running code :::::::::::::::::::::
::::::::::: Note: a new random password will be generated every time :::::::::::

Evaluating args for password length...
No desired password length set in args, using default length of 20
Using password length: 20
Passed complexity requirements
Here you go, a random 20 character password:

N+^70)xOG"H:s,b?796)

Your password contains 8 characters from this list: !"#$%&'()*+,-./:;<=>?@[\]^_`{|}~
Your password contains 5 characters from this list: 0123456789
Your password contains 3 characters from this list: abcdefghijklmnopqrstuvwxyz
Your password contains 4 characters from this list: ABCDEFGHIJKLMNOPQRSTUVWXYZ
Total characters counted: 8 + 5 + 3 + 4 = 20

 */

public class PasswordGenerator {


    private static final String LOWER_CASE = "abcdefghijklmnopqrstuvwxyz";
    private static final String UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static final String NUMBERS = "0123456789";
    private static final String SPECIAL = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
    private static final int MIN = 12;
    private static final int MAX = 2048;
    private static final int defaultPasswordLength = Math.max(MIN, 20);
    private static final int numberOfPasswordShuffles = 3;
    private static final int numberOfTypesShuffles = 3;
    private static final int maxNumberOfShuffles = 100;
    private static final SecureRandom sr = new SecureRandom();

    private static final List<String> characterTypes = new ArrayList<String>();

    static {
        characterTypes.add(secureShuffleString(SPECIAL));
        characterTypes.add(secureShuffleString(LOWER_CASE));
        characterTypes.add(secureShuffleString(UPPER_CASE));
        characterTypes.add(secureShuffleString(NUMBERS));
    }

    public static void main(String[] args) {
        // This is the main function where the code starts to run
        //setDefaultPassLength();

        int passLength = getPasswordLengthFromArgs(args);
        System.out.println("Using password length: " + passLength);
        String pass = generatePassword(passLength);
        System.out.println("Here you go, a random " + pass.length() + " character password: \n\n" + pass);
        countCharacterTypes(pass);
    }


    public static String generatePassword() {
        // This will generate a new password of default length
        return generatePassword(defaultPasswordLength);
    }

    public static String generatePassword(int passLength) {
        // This will generate a new password of your desired length for you
        if (passLength < characterTypes.size()) {
            throw new IllegalArgumentException("Password length is less than the number of character types, increase the password length at least to " + characterTypes.size());
        } else if (passLength < MIN || passLength > MAX) {
            throw new IllegalArgumentException("Either leave the length blank to default to " + defaultPasswordLength + " or choose between " + MIN + " and " + MAX);
        }

        StringBuilder pass = new StringBuilder(passLength);
        guaranteeComplexity(pass);
        secureShuffleList(characterTypes, numberOfTypesShuffles);

        while (pass.length() < passLength) {
            pass.append(getRandomPasswordCharacter()); // build the password
        }

        String finalPassword = Objects.requireNonNull(shuffleStringBuilder(pass, numberOfPasswordShuffles)).toString();
        validateComplexity(finalPassword);
        return finalPassword;
    }

    private static int randNum(int leng) {
        // Picks a random number from 1 to whatever max you want
        return sr.nextInt(leng);
    }

    private static char getRandomPasswordCharacter() {
        // Gets a random character list and then a random character from that list
        String randomType = getRandomCharacterType();
        return getRandomChar(randomType);
    }

    private static char getRandomChar(String str) {
        // This picks a random character from whatever list you sent as 'str'
        int randomCharInd = randNum(str.length());
        return str.charAt(randomCharInd);
    }

    private static String getRandomCharacterType() {
        // Picks a random character list
        int randomTypeInd = randNum(characterTypes.size());
        return characterTypes.get((randomTypeInd));
    }

    public static void guaranteeComplexity(StringBuilder pass) {
        // This ensures the password has something from each character list
        for (String aType : characterTypes) {
            int rn = randNum(aType.length());
            char c = aType.charAt(rn);
            pass.append(c);
        }
    }

    public static void validateComplexity(String pass) {
        // Checks if the password has at least 1 of each character type
        List<String> typesCopy = new ArrayList<>(characterTypes);

        for (int i = 0; i < pass.length(); i++) {
            for (int j = 0; j < typesCopy.size(); j++) {
                if (typesCopy.get(j).indexOf(pass.charAt(i)) != -1) {
                    typesCopy.remove(j);
                    break;
                }
            }
            if (typesCopy.isEmpty()) {
                System.out.println("Passed complexity requirements");
                return;
            }
        }

        throw new Error("ERROR - Password failing complexity requirement, must contain characters from each type");

    }

    public static StringBuilder shuffleStringBuilder(StringBuilder pass, int howManyTimes) {
        // Mixes up all the characters in the password
        if (pass == null) {
            System.out.println("Cannot shuffle null StringBuilder...no shuffling has occurred");
            return null;
        } else if (howManyTimes < 0 || howManyTimes > maxNumberOfShuffles) {
            System.out.println("Cannot shuffle string more than " + maxNumberOfShuffles + " times, you requested this many shuffles: " + howManyTimes
                    + ", only shuffling " + numberOfPasswordShuffles + " times.");
            howManyTimes = numberOfPasswordShuffles;
        }
        for (int i = 0; i < howManyTimes; i++) {
            for (int j = 0; j < pass.length(); j++) {
                int randomIndex = randNum(pass.length());
                char temp = pass.charAt(j);
                pass.setCharAt(j, pass.charAt(randomIndex));
                pass.setCharAt(randomIndex, temp);

            }
        }
        return pass;
    }

    public static String secureShuffleString(String oldString) {

        List<Character> characters = new ArrayList<>();
        for (char c : oldString.toCharArray()) {
            characters.add(c);
        }

        secureShuffleList(characters, numberOfTypesShuffles);
        StringBuilder shuffledString = new StringBuilder();

        for (char c : characters) {
            shuffledString.append(c);
        }

        return shuffledString.toString();
    }

    public static void secureShuffleList(List list, int howManyTimes) {
        // Mixes up the list of character types
        if (list == null) {
            System.out.println("Cannot shuffle null list...no shuffling has occurred");
            return;
        } else if (howManyTimes < 0 || howManyTimes > maxNumberOfShuffles) {
            System.out.println("Cannot shuffle list more than " + maxNumberOfShuffles + ", you requested this many shuffles: " + howManyTimes
                    + ", only shuffling " + numberOfTypesShuffles + " times.");
            howManyTimes = numberOfTypesShuffles;
        }
        for (int i = 0; i < list.size(); i++) {
            int randomIndex = randNum(list.size());
            Object temp = list.get(i);
            list.set(i, list.get(randomIndex));
            list.set(randomIndex, temp);
        }
    }

    public static int getDefaultPasswordLength() {
        // If user doesnt ask for a specific length, use the default password length
        return defaultPasswordLength;
    }

    private static int getPasswordLengthFromArgs(String[] args) {
        // When running as an app, check the arguments to see what length the user asked for
        System.out.println("Evaluating args for password length...");
        if (args == null || args.length == 0) {
            System.out.println("No desired password length set in args, using default length of " + defaultPasswordLength);
        } else if (args.length > 1) {
            throw new IllegalArgumentException("Please pass in only 1 argument as a number for your desired password length");
        } else {
            try {
                System.out.println("Found password length: " + args[0]);
                return Integer.parseInt(args[0]);
            } catch (Exception e) {
                throw new IllegalArgumentException("Error parsing number from arg. Either pass in no args to use the default length of " + defaultPasswordLength + " or pass in a number as an arg");

            }
        }
        return defaultPasswordLength;
    }

    public static void countCharacterTypes(String pass) {
        // Counts how many of each character type exist in the password. Just for fun
        int[] counter = new int[characterTypes.size()];
        for (int i = 0; i < pass.length(); i++) {
            for (int j = 0; j < characterTypes.size(); j++) {
                String type = characterTypes.get(j);
                if (type.contains(pass.charAt(i) + "")) {
                    counter[j]++;
                }
            }
        }

        int totalLength = 0;
        StringBuilder totalMessage = new StringBuilder();
        System.out.println();

        for (int i = 0; i < counter.length; i++) {
            int aLength = counter[i];
            totalLength += aLength;
            System.out.println("Your password contains " + aLength + " characters from this list: " + characterTypes.get(i));
            totalMessage.append(i == counter.length - 1 ? aLength + " = " + totalLength : aLength + " + ");
        }

        System.out.println("Total characters counted: " + totalMessage);
    }
}