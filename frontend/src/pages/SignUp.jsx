import { useForm } from "react-hook-form";
import { useCreateUser } from "../features/user/useUser";

function SignUp() {
  const { register, formState, handleSubmit, reset } = useForm();
  const { errors } = formState;
  const { isCreating, createUser } = useCreateUser();

  function onSubmit({ firstName, lastName, password, phoneNumber }) {
    createUser(
      { firstName, lastName, password, phoneNumber },
      {
        onSettled: () => reset(),
      }
    );
  }

  return (
    <div
      className="flex relative  bg-neutral-200 flex-row   h-screen justify-center p-20 "
      style={{
        backgroundImage: `url('/photo9.jpg')`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    >
      <div className="absolute bottom-30 left-[200px] p-10 bg-white flex flex-col  border rounded-lg  h-2.5/3">
        <form
          action=""
          className="flex flex-col items-center "
          onSubmit={handleSubmit(onSubmit)}
        >
          <input
            type="text"
            id="firstName"
            placeholder=" FirstName"
            className={` w-full p-5  h-10 rounded-lg border outline-none ${
              errors.firstName ? "border-red-500" : ""
            }`}
            {...register("firstName", { required: "This field is required" })}
          />
          {errors.firstName && (
            <p className="text-red-500 test-start text-sm mt-2 w-full">
              {errors.firstName.message}
            </p>
          )}
          <input
            type="text"
            placeholder=" LastName"
            id=" lastName"
            className={` w-full p-5 mt-5 h-10 rounded-lg border outline-none ${
              errors.LastName ? "border-red-500" : ""
            }`}
            {...register("lastName", { required: "This field is required" })}
          />
          {errors.lastName && (
            <p className="text-red-500 test-start text-sm mt-2 w-full">
              {errors.lastName.message}
            </p>
          )}
          <input
            type="number"
            placeholder=" PhoneNumber"
            id=" phoneNumber"
            className={` w-full p-5 mt-5 h-10 rounded-lg border outline-none ${
              errors.phoneNumber ? "border-red-500" : ""
            }`}
            {...register("phoneNumber", { required: "This field is required" })}
          />
          {errors.phoneNumber && (
            <p className="text-red-500 test-start text-sm mt-2 w-full">
              {errors.phoneNumber.message}
            </p>
          )}
          <input
            type="password"
            id="password"
            placeholder=" password"
            className={` w-full p-5 mt-5 h-10 rounded-lg border outline-none ${
              errors.password ? "border-red-500" : ""
            }`}
            {...register("password", { required: "This field is required" })}
          />
          {errors.password && (
            <p className="text-red-500 test-start text-sm mt-2 w-full">
              {errors.password.message}
            </p>
          )}
          <button className="bg-green-600 w-full mt-5  border rounded-lg h-10  text-center text-white ">
            Create Now
          </button>
        </form>
      </div>
    </div>
  );
}

export default SignUp;
