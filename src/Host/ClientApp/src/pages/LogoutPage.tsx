import { useEffect } from "react";
import ErrorField from "../shared/ErrorField";
import LoadingCard from "../shared/LoadingCard";
import DefaultErrorCard from "../shared/DefaultErrorCard";
import { useNavigate } from "react-router-dom";
import { useLogoutMutation } from "../entities/MyUser";

const LogoutPage = () => {
  const navigate = useNavigate();
  const [logout, { isLoading, error, isSuccess }] = useLogoutMutation();

  useEffect(() => {
    logout({});
  }, [logout]);

  useEffect(() => {
    if (isSuccess) {
      navigate('/');
    }
  }, [isSuccess, navigate]);

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error != undefined
          ? <DefaultErrorCard />
          : <div>Выполняется выход из аккаунта...</div>}
    </>
  )
};

export default LogoutPage;