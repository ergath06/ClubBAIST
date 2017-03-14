<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="NewReservation.aspx.cs" Inherits="ClubBAISTWebsite.NewReservation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <div>
    <asp:LoginName ID="LoginName1" runat="server" Font-Bold = "true" />
    <br />
    <asp:LoginStatus ID="LoginStatus1" runat="server" />
    </div>
    <br />
    <br />
    <div>
        <asp:Wizard runat="server" ID="wizAddReservation" DisplaySideBar="false" OnFinishButtonClick="wizAddReservation_FinishButtonClick" OnNextButtonClick="wizAddReservation_NextButtonClick" ActiveStepIndex="0">
            <WizardSteps>
                <asp:WizardStep runat="server" ID="step1">
                    <asp:Label runat="server" ID="labEmail">User: </asp:Label><asp:TextBox runat="server" ID="tbUser" Enabled="false" Visible="true" /><br />
                    <asp:Label runat="server" ID="labNumPlayas">Number of Players: </asp:Label><asp:DropDownList runat="server" ID="ddlNumPlayas">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem></asp:DropDownList><br />
                    <asp:Label runat="server" ID="labNumCarts">Number of Carts: </asp:Label><asp:DropDownList runat="server" ID="ddlNumCarts">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem></asp:DropDownList><br />
                    <asp:Label runat="server" ID="labLocation">Location: </asp:Label><asp:DropDownList runat="server" ID="ddlLocations" DataSourceID="SqlDataSourceLocations" DataTextField="locationName" DataValueField="locationID" />
                    <asp:SqlDataSource ID="SqlDataSourceLocations" runat="server" ConnectionString="<%$ ConnectionStrings:local %>" SelectCommand="spGetLocations" SelectCommandType="StoredProcedure" /><br />
                </asp:WizardStep>
                <asp:WizardStep runat="server" ID="step2">
                    <asp:Calendar runat="server" ID="calReservations" OnDayRender="calReservations_DayRender" OnLoad="calReservations_Load" OnSelectionChanged="calReservations_SelectionChanged" />
                    <asp:Label runat="server" ID="labStartTime" Visible="false">Start Time: </asp:Label><asp:DropDownList runat="server" ID="ddlStartTime" Visible="false" />
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>
        <asp:Label runat="server" ID="labStatus"></asp:Label>
    </div>
</asp:Content>
