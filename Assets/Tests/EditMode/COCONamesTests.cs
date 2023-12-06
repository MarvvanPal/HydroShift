using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class COCONamesTests
{
    private COCONames cocoNames;

    [SetUp]
    public void SetUp()
    {
        cocoNames = new COCONames();
    }

    [TestCase(0, ExpectedResult = "person")]
    [TestCase(1, ExpectedResult = "bicycle")]
    [TestCase(5, ExpectedResult = "bus")]
    [TestCase(13, ExpectedResult = "bench")]
    [TestCase(79, ExpectedResult = "toothbrush")]
    public string GetName_ReturnsCorrectName_ForIndex(int index)
    {
        return cocoNames.GetName(index);
    }
}
