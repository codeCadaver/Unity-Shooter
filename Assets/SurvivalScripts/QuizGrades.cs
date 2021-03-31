using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class QuizGrades : MonoBehaviour
{
    public int quizCount;
    public float minGrade, maxGrade;

    private float _averageGrade;
    
    // Start is called before the first frame update
    void Start()
    {
        // create quiz totalFloat
        float quizTotal = 0.0f;
        // iterate between min and max 
        for (int i = 0; i < quizCount; i++)
        {
            float randomGrade = Random.Range(minGrade, maxGrade);
            Debug.Log($"Quiz grade {i} = {randomGrade}");
            quizTotal += randomGrade;
        }
        
        // divide totalFloat / quizCount
        _averageGrade = quizTotal / quizCount;
        _averageGrade = Mathf.Round(_averageGrade * 100) / 100;
        Debug.Log($"The average grade is: {_averageGrade}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
