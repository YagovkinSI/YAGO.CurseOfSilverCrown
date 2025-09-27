import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, Grid, LinearProgress, Paper, Typography, useMediaQuery, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useEffect } from 'react';
import { useGetCurrentCityQuery } from '../entities/CurrentCity';
import { AccountBalance, Castle, Gavel, MilitaryTech, People } from '@mui/icons-material';

const CurrentCityPage: React.FC = () => {
  const navigate = useNavigate();
  const currentCity = useGetCurrentCityQuery();

  const isLoading = currentCity.isLoading;
  const error = currentCity.error;

  useEffect(() => {
    if (!currentCity.isLoading && currentCity.data == undefined) {
      navigate('/registration');
    }
  }, [currentCity, navigate]);

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  const stats = [
    {
      icon: AccountBalance,
      label: 'Золото',
      value: currentCity.data?.gold ?? 0,
      color: '#FFD700',
      suffix: 'ед.'
    },
    {
      icon: People,
      label: 'Население',
      value: currentCity.data?.population ?? 0,
      color: '#4CAF50',
      suffix: 'чел.'
    },
    {
      icon: MilitaryTech,
      label: 'Армия',
      value: currentCity.data?.military ?? 0,
      color: '#F44336',
      isPercentage: false
    },
    {
      icon: Castle,
      label: 'Укрепления',
      value: currentCity.data?.fortifications ?? 0,
      color: '#795548',
      isPercentage: false
    },
    {
      icon: Gavel,
      label: 'Контроль',
      value: currentCity.data?.control ?? 0,
      color: '#2196F3',
      max: 100,
      isPercentage: true,
      suffix: '%'
    }
  ];

  const renderStatIconAndLabel = (stat: typeof stats[0]) => {
    return (
      <Box display="flex" alignItems="center" mb={1}>
        <stat.icon sx={{ color: stat.color, mr: 1, fontSize: isMobile ? 20 : 24 }} />
        <Typography
          variant={isMobile ? "body2" : "body1"}
          fontWeight="bold"
          color="text.primary"
        >
          {stat.label}
        </Typography>
      </Box>
    )
  }

  const renderStatValue = (stat: typeof stats[0]) => {
    return (
      <Box display="flex" alignItems="center" justifyContent="space-between" mb={1}>
        <Typography
          variant={isMobile ? "h6" : "h5"}
          fontWeight="bold"
          color={stat.color}
        >
          {stat.value}
          {stat.suffix && (
            <Typography
              component="span"
              variant={isMobile ? "body2" : "body1"}
              color="text.secondary"
              ml={0.5}
            >
              {stat.suffix}
            </Typography>
          )}
        </Typography>
      </Box>
    )
  }

  const renderStatLinearProgress = (stat: typeof stats[0]) => {
    return (
      stat.max && (
        <LinearProgress
          variant="determinate"
          value={stat.value}
          sx={{
            height: 6,
            borderRadius: 3,
            backgroundColor: theme.palette.grey[300],
            '& .MuiLinearProgress-bar': {
              backgroundColor: stat.color,
              borderRadius: 3
            }
          }}
        />
      )
    )
  }

  const renderStat = (stat: typeof stats[0]) => {
    return (
      <Paper
        elevation={1}
        sx={{
          p: isMobile ? 1 : 2,
          borderRadius: 2,
          backgroundColor: theme.palette.background.default
        }}
      >
        {renderStatIconAndLabel(stat)}
        {renderStatValue(stat)}
        {renderStatLinearProgress(stat)}
      </Paper>
    )
  }

  const renderContent = () => {
    return (
      <Grid container spacing={isMobile ? 1 : 2}>
        {stats.map((stat) => (
          renderStat(stat)
        ))}
      </Grid>
    )
  }

  const renderCard = () => {
    return (
      <YagoCard
        title={currentCity.data!.name}
        image={`/assets/images/pictures/homepage.jpg`}
      >
        {renderContent()}
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

export default CurrentCityPage