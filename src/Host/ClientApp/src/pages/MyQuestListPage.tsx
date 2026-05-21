import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import YagoButton from '../shared/YagoButton';
import RowData from '../shared/RowData';
import { PriorityHigh } from '@mui/icons-material';
import { useGetMyColonyQuery } from '../entities/MyColony';
import { QuestType, type MyQuest } from '../entities/MyQuest';

const MyQuestListPage: React.FC = () => {
  const navigate = useNavigate();
  const myUserDataResult = useGetMyUserQuery();
  const myColonyResult = useGetMyColonyQuery();

  const isLoading = myUserDataResult.isLoading;
  const error = myUserDataResult.error;

  useEffect(() => {
    if (!(myUserDataResult.data?.data != undefined)) {
      navigate('/registration');
    }
  }, [myUserDataResult, navigate]);

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  const renderQuest = (quest: MyQuest) => {
    const color = quest.type == QuestType.Required
      ? 'red'
      : quest.type == QuestType.Comleted
        ? '#81C784'
        : '#FFD700';
    const url = `/me/quest/${quest.id}`;
    return (<RowData color={color} icon={PriorityHigh} label={quest.title} value={quest.progress} url={url} />)
  }

  const renderCard = () => {
    const quests = myColonyResult.data!.data!.quests;

    return (
      <YagoCard
        title={`Инициативы`}
        image={`/assets/images/pictures/captain_hall.jpg`}
      >
        <Box
          display="flex"
          flexDirection="column"
          gap={1}
          sx={{
            width: '100%',
            maxWidth: isMobile ? 350 : 700,
            margin: '0 auto'
          }}
        >
          {quests.map(q => renderQuest(q))}
        </Box>
        <YagoButton onClick={() => navigate(-1)} type='secondary' >Закрыть</YagoButton>
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

export default MyQuestListPage