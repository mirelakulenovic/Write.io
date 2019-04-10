$(document).ready(function () {
    //Script handling the front-page search bar
    var FPtypingTimer;
    var FPdoneTypingInterval = 150;
    $("#FrontpageSearch").on('keyup', function () {
        clearTimeout(FPtypingTimer);
        FPtypingTimer = setTimeout(FPSearch, FPdoneTypingInterval);
    });
    $("#FrontpageSearch").on('keydown', function () {
        clearTimeout(FPtypingTimer);
    });
    //Script handling the search function in the blog
    $('#BlogSearchbar').on('keypress', function (e) {
        if (e.which === 13) {
            var URL = $(this).data('url');
            var Query = $(this).val();
            window.location.href = URL + Query;

        }
    });
    //Ajax request firing when posting a comment on a blog post
    $('#PostCommentButton').on('click', function (e) {
        var jsonObject = {
            "body": $('#PostComment').val(),
            "PostId": $(this).data("p")
        };
        $.ajax({
            url: "/Blog/PostComment/",
            type: "POST",
            data: JSON.stringify(jsonObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (response) {
                //Executes when something goes wrong. Maybe do something with this?
            },
            success: function (response) {
                //Dito for when it works. Dito on the second part too?
            }
        }).done(function () {
            location.reload();
        });
    });
    //Handles the write blog-post modal
    $('#CreateBlogPost').on('shown.bs.modal', function () {
        var path = window.location.pathname;
        $.ajax({
            url: path + "CreatePost/",
            type: "GET",
        }).done(function (partialViewResult) {
            $("#WritePostContainer").html(partialViewResult);
        });
    });
    //Script that handles creating a post
    $('#CreatePostButton').on('click', function (e) {
        var path = window.location.pathname;
        var jsonModel = {
            "Title": $('#ModelTitle').val(),
            "Body": $('#ModelBody').val(),
            "Tags": $('#ModelTags').val()
        };
        Console.Log(jsonModel.Tags);
        $.ajax({
            url: path + "CreatePost/",
            type: "POST",
            data: JSON.stringify(jsonModel),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (response) {
                //TODO
            },
            success: function (response) {
                //TODO
            }
        }).done(function () {
            location.reload();
        });
    });

    $('#PostComment').on('keyup', function () {
        var Comment = $('#PostComment').val();
        var CharacterCount = Comment.length;
        $('#CharCount').html(`Character count: ${CharacterCount}`);
    });

});

//Front-page search function
//Function that runs when someone stops typing in the search bar
function FPSearch() {
    var filter = $("#FrontpageSearch").val();
    $.ajax({
        url: "/Home/Search/?Query=" + filter,
        type: "GET",
    }).done(function (partialViewResult) {
        $("#HomePageBlogGrid").html(partialViewResult);
    });
};