import { useForm } from "react-hook-form";
import { useLoginUser } from "../features/user/useUser";
import { ClipLoader } from "react-spinners";
import { useEffect } from "react";
function Login() {
  const { register, formState, handleSubmit, reset } = useForm();
  const { errors } = formState;
  const { isLogging, loginUser } = useLoginUser();

  function onSubmit({ phoneNumber, password }) {
    loginUser(
      { phoneNumber, password },
      {
        onSettled: () => reset(),
      }
    );
  }

  return (
    <div
      className="flex relative  bg-neutral-200 flex-row   h-screen  p-20 "
      style={{
        backgroundImage: `url('/photo9.jpg')`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    >
      {isLogging ? (
        <div className="absolute bottom-30 left-[400px] pt-10 pr-10 pl-10 items-center justify-center bg-white flex flex-col border rounded-lg  h-1/5">
          <ClipLoader color="#36d7b7" size={50} />
        </div>
      ) : (
        <div className="absolute bottom-30 left-[200px] pt-10 pr-10 pl-10 bg-white flex flex-col border rounded-lg h-3/5">
          <form
            action=""
            className="flex flex-col items-center "
            onSubmit={handleSubmit(onSubmit)}
          >
            <input
              type="number"
              placeholder="Number"
              id="phoneNumber"
              className={`mt-10  w-full border rounded-lg p-5 h-10 outline-none ${
                errors.number ? "border-red-500" : ""
              }`}
              {...register("phoneNumber", {
                required: "This field is required",
              })}
            />
            {errors.number && (
              <p className="text-red-500 test-start text-sm mt-2 w-full">
                {errors.number.message}
              </p>
            )}
            <input
              type="password"
              id="password"
              placeholder="Password"
              className="  w-full border mt-5 rounded-lg p-5 h-10 outline-none"
              {...register("password", {
                required: "password is required",
                minLength: {
                  value: 8,
                  message: "Password minimum of 8 characters",
                },
              })}
            />
            <p className=" text-start text-sm hover:underline text-green-600 w-full">
              ForgetPassword?
            </p>
            {errors.password && (
              <p className="text-red-500 test-start text-sm mt-2 w-full">
                {errors.password.message}
              </p>
            )}
            <button className="bg-green-600 w-full mt-5  border rounded-lg h-10  text-center text-white">
              Login
            </button>
          </form>
        </div>
      )}
    </div>
  );
}

export default Login;
