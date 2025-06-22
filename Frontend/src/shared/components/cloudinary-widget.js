const cloudName = "ddd2yf0ii"; 
const uploadPreset = "ml_default";

export const cloudinaryWidget = () => {
  return new Promise((resolve, reject) => {
    const uploadedUrls = [];
    const widget = window.cloudinary.createUploadWidget(
      {
        cloudName: cloudName,
        uploadPreset: uploadPreset,
        multiple: true,
        sources: ['local', 'url', 'camera'],
        maxFiles: 10,
      },
      (error, result) => {
        if (error) {
          reject(error);
        } else if (result.event === 'success') {
          uploadedUrls.push(result.info.secure_url);
        } else if (result.event === 'close') {
          resolve(uploadedUrls);
        }
      }
    );

    widget.open();
  });
};
