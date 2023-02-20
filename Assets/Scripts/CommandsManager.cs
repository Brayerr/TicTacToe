using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandsManager : MonoBehaviour
{
    public static event Action<int> OnUndo;
    public static event Action<int, string> OnRedo;

    public static CommandsManager Instance;
    public Stack<int> indexCommands = new Stack<int>();
    public Stack<int> redoIndexCommands = new Stack<int>();
    public Stack<string> redoValueCommands = new Stack<string>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (Instance != this) Destroy(Instance);
    }

    private void Start()
    {
        GameManager.OnUndoCommand += AddUndoCommand;
        GameManager.OnRedoCommand += AddRedoCommand;
        UIManager.OnUndo += UndoCommand;
        UIManager.OnRedo += RedoCommand;
        GameManager.OnRestart += ClearStacks;
    }

    public void AddUndoCommand(int index)
    {
        indexCommands.Push(index);
    }

    public void AddRedoCommand(int index, string value)
    {
        redoIndexCommands.Push(index);
        redoValueCommands.Push(value);
    }

    public void UndoCommand()
    {
        OnUndo.Invoke(indexCommands.Pop());
    }

    public void RedoCommand()
    {
        OnRedo.Invoke(redoIndexCommands.Pop(), redoValueCommands.Pop());
    }

    [ContextMenu("Print All")] 
    public void PrintAll()
    {
        foreach (var item in indexCommands)
        {
            Debug.Log(item);
        }
    }

    public void ClearStacks()
    {
        indexCommands.Clear();
        redoIndexCommands.Clear();
        redoValueCommands.Clear();
    }
}
