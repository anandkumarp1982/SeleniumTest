Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports MySql.Data
Imports MySql.Data.MySqlClient



<TestClass()> Public Class UnitTest1

    Dim connection As New MySqlConnection("Database=companies;Data Source=14.141.46.189;User Id=root;Password=san")
    Dim conn As New MySqlConnection("Database=companies;Data Source=14.141.46.189;User Id=root;Password=san")


    <TestMethod()> Public Sub TestMethod1()
        Using wdriver As IWebDriver = New ChromeDriver()
            wdriver.Navigate().GoToUrl("https://www.zaubacorp.com/")
            wdriver.Manage().Window.Maximize()
            Dim str As String = ""
            Dim CIN As String = ""
            connection.Open()
            Dim command As MySqlCommand = connection.CreateCommand()
            command.CommandText = "select * from Master_companies_list where company_url is null limit 250"
            Dim reader As MySqlDataReader = command.ExecuteReader()
            While reader.Read()
                CIN = reader.GetString(0)
                wdriver.FindElement(By.Id("searchid")).SendKeys(CIN)
                wdriver.FindElement(By.Id("edit-submit--3")).Click()

                conn.Open()
                Dim cmd As New MySqlCommand("update Master_companies_list Set company_url=@company_url where CIN=@CIN", conn)
                cmd.Parameters.AddWithValue("@company_url", wdriver.Url)
                cmd.Parameters.AddWithValue("@CIN", CIN)
                cmd.ExecuteNonQuery()
                cmd.Dispose()
                conn.Close()

            End While
            reader.Close()
            command.Dispose()
            connection.Close()
            'Assert.AreEqual(wdriver.Url, "https://www.zaubacorp.com/")
            wdriver.Quit()
        End Using
    End Sub


End Class