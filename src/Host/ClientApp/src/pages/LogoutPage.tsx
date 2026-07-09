import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useLogoutMutation } from "../entities/MyUser";
import PageContainer from "../shared/PageContainer";

const LogoutPage = () => {
  const navigate = useNavigate();
  const [logout, { isLoading, error, isSuccess }] = useLogoutMutation();

  useEffect(() => {
    logout();
  }, [logout]);

  useEffect(() => {
    if (isSuccess) {
      navigate('/');
    }
  }, [isSuccess, navigate]);

  const renderContent = () => (
    <div>Выполняется выход из аккаунта...</div>
  )

  return (
    <PageContainer backgroundImage='space' isLoading={isLoading} error={error}>
      {renderContent()}
    </PageContainer>
  );
};

export default LogoutPage;