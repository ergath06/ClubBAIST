<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="NewApplication.aspx.cs" Inherits="ClubBAISTWebsite.NewApplication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Primary.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:Label runat="server" ID="labName">Name: </asp:Label><asp:TextBox runat="server" ID="tbName" /><asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="tbName" CssClass="errorText" Text=" Name is Required!" /><br />
    <asp:Label runat="server" ID="labEmail">Email: </asp:Label><asp:TextBox runat="server" ID="tbEmail" /><asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="tbEmail" CssClass="errorText" Text=" Email is Required!" /><asp:RegularExpressionValidator runat="server" ID="regExEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbEmail" CssClass="errorText" Text=" Email is Invalid!"></asp:RegularExpressionValidator><br />
    <asp:Label runat="server" ID="labPass">Password: </asp:Label><asp:TextBox runat="server" ID="tbPass" TextMode="Password" /><asp:RequiredFieldValidator runat="server" ID="rfvPass" ControlToValidate="tbPass" CssClass="errorText" Text=" Password is Required!" /><br />
    <asp:Label runat="server" ID="labPassConfirm">Confirm Password: </asp:Label><asp:TextBox runat="server" ID="tbPassConfirm" TextMode="Password" /><asp:CompareValidator runat="server" ID="compValidPassword" CssClass="errorText" Text=" Passwords don't match!" ControlToValidate="tbPassConfirm" ControlToCompare="tbPass" /><br />
    <asp:Label runat="server" ID="labHomePhone">Home Phone Number: </asp:Label><asp:TextBox runat="server" ID="tbHomePhone" TextMode="Phone" /><br />
    <asp:Label runat="server" ID="labCellPhone">Cell Phone Number: </asp:Label><asp:TextBox runat="server" ID="tbCellPhone" TextMode="Phone" /><br />
    
    <asp:Label runat="server" ID="labMembershipType">Select Requested Membership Type: </asp:Label><asp:DropDownList runat="server" ID="ddlMemberships" DataSourceID="SqlDataSource1" DataTextField="levelTitle" DataValueField="membershipID" />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:local %>" SelectCommand="spGetMembershipTypes" SelectCommandType="StoredProcedure" />
    <br />
    <asp:Button runat="server" ID="butSub" Text="Submit" OnClick="butSub_Click" /><asp:Label ID="labMessage" runat="server" Text="Some message text goes here!" Visible="false" />
</asp:Content>
