function UpdateApp() {
  return (
    <div className="justify-center flex dark:bg-cyan-950">
      <div className="bg-white text-White w-2/3 rounded-lg dark:bg-cyan-700 dark:text-white">
        <p className="mt-3 font-bold">Update</p>
        <p className="text-green-800 mb-4 dark:text-white">WhatsApp</p>
        <p className="text-gray-800 mb-4 text-sm dark:text-white">
          New Version Available
        </p>
        <button className="bg-green-800 rounded-md p-3 mb-3 text-white">
          Update now
        </button>
      </div>
    </div>
  );
}

export default UpdateApp;
