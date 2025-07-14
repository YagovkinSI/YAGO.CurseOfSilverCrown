import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import { useGetCurrentUserQuery } from '../entities/CurrentUser';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';

const GamePage: React.FC = () => {
  const navigate = useNavigate();
  const { data, isLoading, error } = useGetCurrentUserQuery();

  useEffect(() => {
    if (!isLoading && !error && data?.isAuthorized === false) {
      navigate('/Identity/Account/Register');
    }
  }, [data, isLoading, error, navigate]);

  const renderCard = () => {
    return (
      <YagoCard
        title={`Господа`}
        image={'/assets/images/pictures/home.jpg'}
      >
        <Typography textAlign="justify" gutterBottom>
          Магистр Илтарин редко удостаивал его словами. Старший сын, Кэлан, лишь отдавал приказы. Лишь младшая, Лира, иногда шептала: «Не бойся». Говорила она это больше для себя — её пальцы сжимали подол платья, когда за окном раздавались чужие голоса. Но мальчик и не боялся. Просто ждал, когда этот город станет хоть немного понятным.
        </Typography>
        <ButtonWithLink to={''} text={'Далее'} />
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error != undefined
          ? <DefaultErrorCard />
          : renderCard()}
    </>
  )
}

export default GamePage