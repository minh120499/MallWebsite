function getErrorsMessage(error) {
  if (error?.data?.error) {
    return error?.data?.error;
  }

  if (!error?.data?.errors[0]) {
    return "";
  }
  return Object.values(error.data.errors[0]);
};

function formattedDate(input) {
  const date = new Date(input);
  if (!input || date === "Invalid Date") return "";
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');

  return `${hours}:${minutes} ${day}-${month}-${year}`;
};

function timeDifference(start, end) {
  var startTime = new Date('2023-07-20T12:00:00');
  var endTime = new Date('2023-07-21T14:30:00');
  let a = "1"
  var timeDifference = endTime - currentTime;
  // Tính số ngày, giờ, phút, giây từ chênh lệch thời gian
  var days = Math.floor(timeDifference / (1000 * 60 * 60 * 24));
  var hours = Math.floor((timeDifference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
  var minutes = Math.floor((timeDifference % (1000 * 60 * 60)) / (1000 * 60));
  var seconds = Math.floor((timeDifference % (1000 * 60)) / 1000);


  var countdownInterval = setInterval(function () {
    return countdown(endTime);
  }, 1000);

  return a
};

function renderImage(imageUrl) {
  if (!imageUrl) return
  const images = $('.images')
  images.innerHtml = ''
  images.prepend('<div class="img" style="background-image: url(\'' + imageUrl + '\');" rel="' + imageUrl + '"><span>remove</span></div>')
  images.on('click', '.img', function () {
    $(this).remove()
  })
}