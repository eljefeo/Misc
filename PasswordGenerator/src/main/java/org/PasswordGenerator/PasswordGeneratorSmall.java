package org.PasswordGenerator;

import java.security.SecureRandom;

public class PasswordGeneratorSmall {
    private static final String LOWER_CASE = "abcdefghijklmnopqrstuvwxyz";
    private static final String UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static final String NUMBERS = "0123456789";
    private static final String SPECIAL = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
    private static final String ALL_CHARS = LOWER_CASE + UPPER_CASE + NUMBERS + SPECIAL;
    private static final int PASS_LENGTH = 20;
    private static final int MIN_LENGTH = 12;
    private static final SecureRandom sr = new SecureRandom();

    public static void main(String[] args) {
        // Main function to run locally
        System.out.println("Using password length: " + PASS_LENGTH);
        String pass = generatePassword(PASS_LENGTH);
        System.out.println("Here you go, a random " + pass.length() + " character password: \n\n" + pass);
    }

    public static String generatePassword(int passLength) {
        // This will generate a new password of your desired length for you
        if (passLength < MIN_LENGTH) {
            throw new IllegalArgumentException("Password length is too small, increase the password length at least to " + MIN_LENGTH);
        }

        StringBuilder pass = new StringBuilder(passLength);

        //First add 1 of each type of character to guarantee there is at least lower case, upper case, number, and special
        pass.append(LOWER_CASE.charAt(sr.nextInt(LOWER_CASE.length())));
        pass.append(UPPER_CASE.charAt(sr.nextInt(UPPER_CASE.length())));
        pass.append(NUMBERS.charAt(sr.nextInt(NUMBERS.length())));
        pass.append(SPECIAL.charAt(sr.nextInt(SPECIAL.length())));

        // Keep adding random chars until we get the desired length
        while (pass.length() < passLength) {
            pass.append(ALL_CHARS.charAt(sr.nextInt(ALL_CHARS.length()))); // build the password
        }

        //Shuffle the new passsword
        for (int j = 0; j < pass.length(); j++) {
            int randomIndex = sr.nextInt(pass.length());
            char temp = pass.charAt(j);
            pass.setCharAt(j, pass.charAt(randomIndex));
            pass.setCharAt(randomIndex, temp);
        }

        return pass.toString();
    }
}