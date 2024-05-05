package org.example;

import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.List;

public class PasswordGenerator {

    private static final int defaultPasswordLength = 200;
    private static final String LOWER_CASE = "abcdefghijklmnopqrstuvwxyz";
    private static final String UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static final String NUMBERS = "0123456789";
    private static final String SPECIAL = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
    private static final int MIN = 12;
    private static final int MAX = 2048;
    private static final int numberOfPasswordShuffles = 3;
    private static final int numberOfTypesShuffles = 3;
    private static final int maxNumberOfShuffles = 100;
    private static final SecureRandom sr = new SecureRandom();

    private static final List<String> characterTypes = new ArrayList<String>();
    static {
        characterTypes.add(SPECIAL);
        characterTypes.add(LOWER_CASE);
        characterTypes.add(UPPER_CASE);
        characterTypes.add(NUMBERS);
    }

    public static void main(String[] args) {
        int passLength = getPasswordLengthFromArgs(args);
        System.out.println("Using password length: " + passLength);
        String pass = generatePassword(passLength);
        System.out.println("Here you go, a random " + pass.length() + " character password: \n\n" + pass);
        countCharacterTypes(pass);

        //String defaultLengthPass = generatePassword();
        //System.out.println("Default length password: " + defaultLengthPass);

    }

    public static String generatePassword(){
        return generatePassword(defaultPasswordLength);
    }

    public static String generatePassword(int passLength) {
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

        String finalPassword = shuffleStringBuilder(pass, numberOfPasswordShuffles).toString();
        validateComplexity(finalPassword);
        return finalPassword;
    }

    private static int randNum(int leng){
        return sr.nextInt(leng);
     }

     private static char getRandomPasswordCharacter(){
        String randomType = getRandomCharacterType();
        return getRandomChar(randomType);
     }

     private static char getRandomChar(String str){
        int randomCharInd = randNum(str.length());
        return str.charAt(randomCharInd);
     }



     private static String getRandomCharacterType(){
        int randomTypeInd = randNum(characterTypes.size());
        return characterTypes.get((randomTypeInd));
     }

     public static void guaranteeComplexity(StringBuilder pass){
        for(String aType: characterTypes){
            int rn = randNum(aType.length());
            char c = aType.charAt(rn);
            pass.append(c);
        }
     }

     public static void validateComplexity(String pass){
         List<String> typesCopy = new ArrayList<>(characterTypes);

        for(int i = 0; i < pass.length(); i++){
            for(int j = 0; j < typesCopy.size(); j++){
                if(typesCopy.get(j).indexOf(pass.charAt(i)) != -1){
                    typesCopy.remove(j);
                    break;
                }
            }
            if(typesCopy.isEmpty()){
                System.out.println("Passed complexity requirements");
                return;
            }
        }

        throw new Error("ERROR - Password failing complexity requirement, must contain characters from each type");

     }

     public static StringBuilder shuffleStringBuilder(StringBuilder pass, int howManyTimes){
         if(pass == null){
             System.out.println("Cannot shuffle null StringBuilder...no shuffling has occurred");
             return null;
         } else if(howManyTimes < 0 || howManyTimes > maxNumberOfShuffles) {
             System.out.println("Cannot shuffle string more than " + maxNumberOfShuffles + " times, you requested this many shuffles: " + howManyTimes
                     + ", only shuffling " + numberOfPasswordShuffles + " times." );
             howManyTimes = numberOfPasswordShuffles;
         }
         for(int i = 0; i < howManyTimes; i++) {
             for (int j = 0; j < pass.length(); j++) {
                 int randomIndex = randNum(pass.length());
                 char temp = pass.charAt(j);
                 pass.setCharAt(j, pass.charAt(randomIndex));
                 pass.setCharAt(randomIndex, temp);

             }
         }
         return pass;
     }

     public static void secureShuffleList(List list, int howManyTimes){
        if(list == null){
            System.out.println("Cannot shuffle null list...no shuffling has occurred");
            return;
        }else if(howManyTimes < 0 || howManyTimes > maxNumberOfShuffles) {
            System.out.println("Cannot shuffle list more than " + maxNumberOfShuffles + ", you requested this many shuffles: " + howManyTimes
                    + ", only shuffling " + numberOfTypesShuffles + " times." );
            howManyTimes = numberOfTypesShuffles;
        }
         for(int i = 0; i < list.size(); i++){
             int randomIndex = randNum(list.size());
             Object temp = list.get(i);
             list.set(i, list.get(randomIndex));
             list.set(randomIndex, temp);
         }
     }

     public static int getDefaultPasswordLength(){
        return defaultPasswordLength;
     }

    private static int getPasswordLengthFromArgs(String[] args) {
        System.out.println("Evaluating args for password length..");
        if(args == null || args.length == 0){
            System.out.println("No desired password length set in args, using default length of " + defaultPasswordLength);
        } else if(args.length > 1){
            throw new IllegalArgumentException("Please pass in only 1 argument as a number for your desired password length");
        } else if (args.length == 1) {
            try{
                System.out.println("Found password length: " + args[0]);
                return Integer.parseInt(args[0]);
            } catch (Exception e){
                throw new IllegalArgumentException("Error parsing number from arg. Either pass in no args to use the default length of " + defaultPasswordLength + " or pass in a number as an arg");

            }
        }
        return defaultPasswordLength;
    }

    public static void countCharacterTypes(String pass){
        int[] counter = new int[characterTypes.size()];
        for(int i = 0; i < pass.length(); i++){
            for(int j = 0; j < characterTypes.size(); j++){
                String type = characterTypes.get(j);
                if(type.contains(pass.charAt(i) + "")){
                    counter[j]++;
                }
            }
        }

        int totalLength = 0;
        String totalMessage = "";
        System.out.println();

        for (int i = 0; i < counter.length; i++) {
            int aLength = counter[i];
            totalLength += aLength;
            System.out.println("Your password contains " + aLength + " characters from this list: " + characterTypes.get(i));
            totalMessage += i == counter.length-1 ? aLength + " = " + totalLength : aLength + " + ";
        }

        System.out.println("Total characters counted: " + totalMessage);
    }
}