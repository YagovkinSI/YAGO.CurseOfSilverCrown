import { useEffect } from "react";
import { useLogoutMutation } from "../entities/CurrentUser";
import ErrorField from "../shared/ErrorField";
import LoadingCard from "../shared/LoadingCard";
import DefaultErrorCard from "../shared/DefaultErrorCard";
import { useNavigate } from "react-router-dom";

const LogoutPage = () => {
  const navigate = useNavigate();
  const [loginMutate, { isLoading, error }] = useLogoutMutation();

  useEffect(() => {
    loginMutate();
    navigate(-1);
  });

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error != undefined
          ? <DefaultErrorCard />
          : <></>}
    </>
  )
};

export default LogoutPage;