using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetParser : MonoBehaviour
{
    public static string splitter = "-##|}{>/<:";
    public static string parser = "+||**@#4~`";

    /// <summary>
    /// Splits a message. Network will recognize as seperate messages.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string[] Split(string message)
    {
        List<string> words = new List<string>();
        char[] letters = message.ToCharArray();
        int wordsIndex = 0;

        for(int i = 0; i < letters.Length; i++)
        {
            if (letters[i] != splitter[0])
            {
                NetParser.AddToList(letters[i], wordsIndex, words);
            }
            else
            {
                bool issplitter = isSplitter(i, message);
                if (issplitter)
                {
                    i += splitter.Length-1;
                    wordsIndex++;
                }
                else
                {
                    NetParser.AddToList(letters[i], wordsIndex, words);
                }
            }

        }
        foreach(string word in words)
        {
            word.Replace(NetParser.splitter, "");

        }

        return words.ToArray();
    }

    protected static bool isSplitter(int curIndex,string message)
    {
        if (message.Length >= curIndex + splitter.Length)
        {
            string output = message.Substring(curIndex, splitter.Length);

            return output == splitter;
        }
        else
        {
            return false;
        }

    
    }

    protected static bool isParser(int curIndex, string message)
    {
        if (message.Length >= curIndex + parser.Length)
        {
            string output = message.Substring(curIndex, parser.Length);

            return output == parser;
        }
        else
        {
            return false;
        }
            
    }

    protected static void AddToList(char letter,int index, List<string> list)
    {
        while(list.Count <= index) {

            list.Add("");

        }
        list[index] += letter;

    }

    public static string Join(params string[] messages)
    {
        string finalMessage = "";
        foreach(string message in messages)
        {
            finalMessage += message + splitter;
        }

        return finalMessage;
    }

    public static string JoinForParse(params string[] messages)
    {
        string finalMessage = "";
        foreach (string message in messages)
        {
            finalMessage += message + parser;
        }

        return finalMessage;
    }

    /// <summary>
    /// Parses a message. Network will not recognize as separate messages.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string[] Parse(string message)
    {
        
        List<string> words = new List<string>();
        char[] letters = message.ToCharArray();
        int wordsIndex = 0;
        
        for (int i = 0; i < letters.Length; i++)
        {
            if (letters[i] != parser[0])
            {
                NetParser.AddToList(letters[i], wordsIndex, words);
            }
            else
            {
                bool issplitter = isParser(i, message);
                if (issplitter)
                {
                    i += parser.Length - 1;
                    wordsIndex++;
                }
                else
                {
                    NetParser.AddToList(letters[i], wordsIndex, words);
                }
            }

        }
        
        foreach (string word in words)
        {
            word.Replace(NetParser.parser, "");

        }
        
        return words.ToArray();
    }


}
