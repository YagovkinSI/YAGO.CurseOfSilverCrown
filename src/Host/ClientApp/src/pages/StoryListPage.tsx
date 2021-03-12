import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Card, CardActions, CardHeader, Pagination } from '@mui/material';
import { useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useGetStoryListQuery, type StoryItem } from '../entities/StoryList';
import ButtonWithLink from '../shared/ButtonWithLink';
import YagoAvatar from '../shared/YagoAvatar';

const StoryListPage: React.FC = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const storyListResult = useGetStoryListQuery(currentPage);

  const isLoading = storyListResult.isLoading;
  const error = storyListResult.error;

  const handlePageChange = (_event: React.ChangeEvent<unknown>, page: number) => {
    setCurrentPage(page);
  };

  const renderStoryItem = (item: StoryItem) => {
    return (
      <Card sx={{marginBottom: '0.5rem'}}>
        <CardHeader sx={{textAlign: 'left'}}
          avatar={<YagoAvatar name={item.user.name} />}
          title={`${item.chapter}. ${item.title}`}
          subheader={item.user.name}
        />
        <CardActions sx={{justifyСontent: 'center'}}>
          <ButtonWithLink key={item.id} to={`/story?id=${item.id}`} text={'Читать'} />
        </CardActions>
      </Card>
    )
  }

  const renderPagination = () => {
    const pageCount = storyListResult.data
      ? Math.ceil(storyListResult.data.total / storyListResult.data.limit)
      : 0;

    return (
      <Pagination
        count={pageCount}
        page={currentPage}
        onChange={handlePageChange}
        variant="outlined"
        shape="rounded"
        sx={{ mt: 3 }}
      />
    )
  }

  const renderCard = () => {
    const storyList = storyListResult.data!;
    return (
      <YagoCard
        title={'Истории игроков'}
        image={undefined}
      >
        {storyList.data.map(s => renderStoryItem(s))}
        {storyList.total > storyList.limit && renderPagination()}
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

export default StoryListPage