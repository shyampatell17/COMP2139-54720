$(document).ready(function () {
    var projectId = $('#projectComments input[name="ProjectId"]').val();

    loadComments(projectId);

    $('#addCommentForm').submit(function (e) {
        e.preventDefault();

        var formData = {
            ProjectId: projectId,
            content: $('#projectComments textarea[name="Content"]').val()
        };

        $.ajax({
            url: '/ProjectManagement/ProjectComment/AddComments',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $('#projectComments textarea[name="Content"]').val('');
                    loadComments(projectId);
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Error: " + error);
            }
        });
    });
});


function loadComments(projectId) {
    $.ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId,
        method: 'GET',
        success: function (data) {
            var commentHtml = '';
            for (var i = 0; i < data.length; i++) {
                commentHtml += '<div class="comment">';
                commentHtml += '<p>' + data[i].content + '</p>';
                commentHtml += '<span>Posted on ' + new Date(data[i].datePosted).toLocaleDateString() + '</span>';
                commentHtml += '</div>';
            }
            $('#commentsList').html(commentHtml);
        }
    });
}

