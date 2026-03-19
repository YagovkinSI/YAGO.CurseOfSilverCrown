import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';
import { useCreateTemporaryUserMutation, useGetMyUserQuery, type MyUser } from '../entities/MyUser';
import TextFooterComment from '../shared/TextFooterComment';
import { useGetMyColonyQuery } from '../entities/MyColony';

const HomePage: React.FC = () => {
  const getMyUserResult = useGetMyUserQuery();
  const getMyColonyResult = useGetMyColonyQuery();
  const [createTemporaryUser, createTemporaryUserResult] = useCreateTemporaryUserMutation();
  const navigate = useNavigate();

  const isLoading = getMyUserResult.isLoading || getMyColonyResult.isLoading || createTemporaryUserResult.isLoading;
  const error = getMyUserResult.error ?? getMyColonyResult.error ?? createTemporaryUserResult.error;

  const user = getMyUserResult.data?.data;
  const colony = getMyColonyResult.data?.data;

  const autoRegisterAndGame = () => {
    createTemporaryUser()
      .unwrap()
      .then(() => navigate('/me/colony'));
  }

  const renderGuestContent = () => {
    return (
      <>
        <YagoButton onClick={autoRegisterAndGame} text={'Быстрый старт'} isDisabled={false} />
        <ButtonWithLink to={'/registration'} text={'Войти / Регистрация'} />
      </>
    )
  }

  const renderAuthorizedUserContent = (user: MyUser) => {
    const buttonName = colony == undefined
      ? 'Создать колонию'
      : user.isTemporary
        ? `Продолжить как ${user.userName}`
        : `В колонию ${colony.name}`

    return (
      <>
        {user.isTemporary && <ButtonWithLink to={'/registration'} text={'Изменить имя и пароль'} />}
        <ButtonWithLink to={'/me/colony'} text={buttonName} />
      </>
    )
  }

  const renderCard = () => {
    return (
      <YagoCard
        title={`Мир YAGO`}
        image={'/assets/images/pictures/homepage.jpg'}
        headerButtonsAccess={false}
      >
        <Typography textAlign="center" gutterBottom>
          Каким будет твоё государство среди звёзд?
        </Typography>
        {user != undefined
          ? renderAuthorizedUserContent(user)
          : renderGuestContent()}
        <TextFooterComment>
          Для создания визуального и текстового контента в этой игре в качестве инструмента прототипирования и вдохновения использовались технологии искусственного интеллекта. Финальный творческий отбор и интеграция выполнены разработчиком. Мы с уважением относимся к творчеству художников и писателей по всему миру.
        </TextFooterComment>
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : renderCard()}
    </>
  )
}

export default HomePage