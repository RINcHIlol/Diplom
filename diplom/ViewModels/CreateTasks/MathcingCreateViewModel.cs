using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;

namespace diplom.ViewModels.CreateTasks;

public class MatchingCreateViewModel : TaskCreateViewModel
{
    public ObservableCollection<AnswerOption> Left { get; } = new();
    public ObservableCollection<AnswerOption> Right { get; } = new();
    
    public List<(int LeftIndex, int RightIndex)> Pairs { get; } = new();

    public MatchingCreateViewModel()
    {
        Left.Add(new AnswerOption());
        Right.Add(new AnswerOption());
    }
    
    public void AddPair(int leftIndex, int rightIndex)
    {
        if (!Pairs.Any(p => p.LeftIndex == leftIndex))
            Pairs.Add((leftIndex, rightIndex));
    }

    public void AddLeft() => Left.Add(new AnswerOption());
    public void AddRight() => Right.Add(new AnswerOption());

    public void RemoveLeft(AnswerOption item)
    {
        if (Left.Count > 1)
            Left.Remove(item);
    }

    public void RemoveRight(AnswerOption item)
    {
        if (Right.Count > 1)
            Right.Remove(item);
    }
    
    public override CreateTaskDto BuildDto()
    {
        var answers = new List<CreateAnswerDto>();

        int index = 0;

        foreach (var a in Left)
        {
            answers.Add(new CreateAnswerDto
            {
                AnswerText = a.Text,
                OrderIndex = index++
            });
        }

        foreach (var a in Right)
        {
            answers.Add(new CreateAnswerDto
            {
                AnswerText = a.Text,
                OrderIndex = index++
            });
        }

        return new CreateTaskDto
        {
            TaskTypeId = 4,
            Question = Question,
            Answers = answers,
            MatchingPairs = null // важно
        };
    }
    
    // public override CreateTaskDto BuildDto()
    // {
    //     var answers = new List<CreateAnswerDto>();
    //     var map = new Dictionary<int, int>(); // TempId → OrderIndex
    //
    //     int index = 0;
    //
    //     foreach (var a in Left)
    //     {
    //         map[a.TempId] = index;
    //
    //         answers.Add(new CreateAnswerDto
    //         {
    //             AnswerText = a.Text,
    //             OrderIndex = index++
    //         });
    //     }
    //
    //     foreach (var a in Right)
    //     {
    //         map[a.TempId] = index;
    //
    //         answers.Add(new CreateAnswerDto
    //         {
    //             AnswerText = a.Text,
    //             OrderIndex = index++
    //         });
    //     }
    //
    //     // пары через TempId
    //     var pairs = new List<CreateMatchingPairDTO>();
    //
    //     int count = Math.Min(Left.Count, Right.Count);
    //
    //     // for (int i = 0; i < count; i++)
    //     // {
    //     //     // pairs.Add(new CreateMatchingPairDTO
    //     //     // {
    //     //     //     LeftId = map[Left[i].TempId],
    //     //     //     RightId = map[Right[i].TempId]
    //     //     // });
    //     //     pairs = Pairs.Select(p => new CreateMatchingPairDTO
    //     //     {
    //     //         LeftId = map[Left[p.LeftIndex].TempId],
    //     //         RightId = map[Right[p.RightIndex].TempId]
    //     //     }).ToList();
    //     // }
    //     
    //     pairs = Pairs.Select(p => new CreateMatchingPairDTO
    //     {
    //         LeftId = map[Left[p.LeftIndex].TempId],
    //         RightId = map[Right[p.RightIndex].TempId]
    //     }).ToList();
    //
    //     return new CreateTaskDto
    //     {
    //         TaskTypeId = 4,
    //         Question = Question,
    //         Answers = answers,
    //         MatchingPairs = pairs
    //     };
    // }
    
    public override void LoadFromDto(TaskDto task)
    {
        Left.Clear();
        Right.Clear();
        Pairs.Clear();

        var answers = task.Answers.OrderBy(a => a.OrderIndex).ToList();
        int half = answers.Count / 2;

        // 🔹 создаём mapping AnswerId → index
        var idToIndex = new Dictionary<int, int>();

        // LEFT
        for (int i = 0; i < half; i++)
        {
            Left.Add(new AnswerOption
            {
                Text = answers[i].AnswerText
            });

            idToIndex[answers[i].Id] = i;
        }

        // RIGHT
        for (int i = half; i < answers.Count; i++)
        {
            Right.Add(new AnswerOption
            {
                Text = answers[i].AnswerText
            });

            idToIndex[answers[i].Id] = i;
        }

        // 🔥 ВОССТАНАВЛИВАЕМ ПАРЫ
        if (task.MatchingPairs != null)
        {
            foreach (var p in task.MatchingPairs)
            {
                if (idToIndex.ContainsKey(p.LeftId) && idToIndex.ContainsKey(p.RightId))
                {
                    var leftIndex = idToIndex[p.LeftId];
                    var rightIndex = idToIndex[p.RightId] - half;

                    Pairs.Add((leftIndex, rightIndex));
                }
            }
        }

        Question = task.Question;
    }

    // public override CreateTaskDto BuildDto()
    // {
    //     var answers = new List<CreateAnswerDto>();
    //
    //     int index = 0;
    //
    //     answers.AddRange(Left.Select(a => new CreateAnswerDto
    //     {
    //         AnswerText = a.Text,
    //         OrderIndex = index++
    //     }));
    //
    //     answers.AddRange(Right.Select(a => new CreateAnswerDto
    //     {
    //         AnswerText = a.Text,
    //         OrderIndex = index++
    //     }));
    //
    //     Pairs.Clear();
    //
    //     int count = Math.Min(Left.Count, Right.Count);
    //
    //     for (int i = 0; i < count; i++)
    //     {
    //         Pairs.Add((i, Left.Count + i));
    //     }
    //
    //     return new CreateTaskDto
    //     {
    //         TaskTypeId = 4,
    //         Question = Question,
    //         Answers = answers,
    //
    //         MatchingPairs = Pairs.Select(p => new CreateMatchingPairDTO
    //         {
    //             LeftIndex = p.LeftIndex,
    //             RightIndex = p.RightIndex
    //         }).ToList()
    //     };
    // }
}