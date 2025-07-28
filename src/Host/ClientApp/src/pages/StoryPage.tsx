import vk_logo from '../assets/images/links/vk_logo.svg'
import YagoCard from '../shared/YagoCard';
import { Typography } from '@mui/material';
import React from 'react';
import SwgWithLink from '../shared/SwgWithLink';

const StoryPage: React.FC = () => {

  return (
    <>
      <YagoCard
        title={'Пока в разработке...'}
        image={`/favicon.png`}
      >
        <Typography gutterBottom>О процессе разработки можно почитать в группе ВК.</Typography>
        <SwgWithLink url="https://vk.com/club189975977" swgPath={vk_logo} alt="vk link" />
      </YagoCard>
    </>
  )
}

export default StoryPage