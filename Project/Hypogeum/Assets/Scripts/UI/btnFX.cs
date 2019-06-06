/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.UI;

public class btnFX : MonoBehaviour
{

    public Color cursorInsideColor = new Color(200.0f / 255.0f, 200.0f / 255.0f, 200.0f / 255.0f, 128.0f / 255.0f);
    public Color cursorOutsideColor = new Color(255.0f / 255.0f, 255.0f / 0.0f, 255.0f / 255.0f, 255.0f / 255.0f);
    //public Button button;
    public Text text;

    public void OnCursorEnter()
    {
        //button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        text.color = cursorInsideColor;
    }

    public void onCursorExit()
    {
        //button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        text.color = cursorOutsideColor;
    }


}