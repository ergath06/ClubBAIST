<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="DeleteReservations.aspx.cs" Inherits="ClubBAISTWebsite.DeleteReservations" %>
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
    <asp:Wizard runat="server" ID="wizSearchReservation" DisplaySideBar="false" ActiveStepIndex="0" OnFinishButtonClick="wizSearchReservation_FinishButtonClick" OnNextButtonClick="wizSearchReservation_NextButtonClick">
        <WizardSteps>
        <asp:WizardStep>
            <asp:Label runat="server" ID="labUser">User: </asp:Label><asp:TextBox runat="server" ID="tbUser" /> <br />
            <asp:Label runat="server" ID="labLocation">Location: </asp:Label><asp:DropDownList runat="server" ID="ddlLocations" DataSourceID="SqlDataSourceLocations" DataTextField="locationName" DataValueField="locationID" />
            <asp:SqlDataSource ID="SqlDataSourceLocations" runat="server" ConnectionString="<%$ ConnectionStrings:local %>" SelectCommand="spGetLocations" SelectCommandType="StoredProcedure" /> 
        </asp:WizardStep>
            <asp:WizardStep>
                <asp:Label runat="server" ID="labStartDate">Search Start Date: </asp:Label><br />
                <asp:Calendar runat="server" ID="calStartDate" OnDayRender="calStartDate_DayRender" />
            </asp:WizardStep>
            <asp:WizardStep>
                <asp:Label runat="server" ID="labEndDate">Search End Date: </asp:Label><br />
                <asp:Calendar runat="server" ID="calEndDate" OnDayRender="calEndDate_DayRender" />
            </asp:WizardStep>
    </WizardSteps>
    </asp:Wizard>
    <br />
    <asp:Label runat="server" ID="labReservationID" Visible="false">Reservation ID: </asp:Label><asp:DropDownList runat="server" ID="ddlReservations" Visible="false" /> <asp:Button runat="server" ID="butDelete" Text="Delete" Visible="false" OnClick="butDelete_Click" /> <br />
    <asp:Label runat="server" ID="labMessage"></asp:Label>
    <br />
    <br />
    <asp:Table ID="OutputTable" runat="server" GridLines="Both">
    </asp:Table>
</asp:Content>
