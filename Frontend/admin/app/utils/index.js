function getErrorsMessage(error) {
  if (!error?.data?.errors[0]) {
    return "";
  }
  return Object.values(error.data.errors[0]);
};