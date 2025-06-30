import * as React from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import type HistoryEvent from '../entities/HistoryEvent';
import { ToGameDate } from '../features/GameDateCreator';
import { Box } from '@mui/material';

export interface HistoryEventProps {
  event: HistoryEvent
}

const HistoryEventCard: React.FC<HistoryEventProps> = (props) => {

  const renderEventDate = () => {
    const date = new Date(props.event.dateTime);

    return (
      <Box sx={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        width: '100%'
      }}>
        <Typography variant="body2" >
          {ToGameDate(props.event.dateTime)}
        </Typography>
        <Typography variant="body2" sx={{ color: 'text.secondary' }} >
          {date > new Date('2025-01-01') ? date.toLocaleString() : ''}
        </Typography>
      </Box>
    )
  }

  return (
    <Card sx={{ minWidth: 275, textAlign: 'left', marginBottom: '0.5rem' }}>
      <CardContent sx={{ padding: '16px 16px 0 16px' }} >
        {renderEventDate()}
        <Typography variant="body1">
          {props.event.shortText}
        </Typography>
      </CardContent>
      {/* {<CardActions>
        <Button disabled size="small">Подробнее</Button>
      </CardActions>} */}
    </Card>
  );
}

export default HistoryEventCard;