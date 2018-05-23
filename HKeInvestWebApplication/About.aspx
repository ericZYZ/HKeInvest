<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="HKeInvestWebApplication.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Additional Feature: Favorite List</h3>
    <h4>Description Page</h4>
    <div>
        <h5>Overview:</h5>
        <p>
            The favorite list feature provides the service for HKeInvest clients to store their own favorite securities so that they 
            can view their favorite secirities every time by easily clicking a button to display. In addition, clients can easily add 
            and delete items into/from their lists.
        </p>
        <br />
        <h5>Usage:</h5>
        <p>
            The service can only be accessed by HKeInvest clients. To use the service, a client need to log in as client, and open 
            the ClientOnly/FavoriteList.aspx page.
        </p>
        <p>
            In the page, the upper part is provides the security searching service for user's convenience, while the lower part is the 
            management part of the favorite list. 
        </p>
        <p>
            1. Security searching: It is approximately the same as the normal security searching function.
        </p>
        <p>
            2. Favorite list management: 
        </p>
        <p>
            To display the list, click the radio button "Display/Delete", and then the favorite list of the user will appears, 
            if any. Meanwhile, there are link buttons at the end of every row, namely the delete buttons, by clickng wich the 
            user can remove the item from his list.
        </p>
        <p>
            To add item, click the radio button "Add", and a dropdown list and a text box will appear along with an "Add" button. 
            User need to select a type of security from the dropdown list and input the code of a security, and click the "Add" button. 
            An message indicating failure will appear if either the input is invalid (empty selection, empty code), or the input doesn't 
            match with any security. Otherwise, a success message will show up instead, and the item is added successfully.
        </p>
    </div>
</asp:Content>
